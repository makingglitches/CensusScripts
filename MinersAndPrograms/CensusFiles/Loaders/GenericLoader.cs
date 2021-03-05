using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;


using DbfDataReader;

namespace CensusFiles.Loaders
{
    public class GenericLoader
    {


        public GenericLoader(LoaderOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Delegate representing the thingamajig that creates new class instances.
        /// </summary>
        /// <returns></returns>
        public delegate IRecordLoader NewClass();
        public delegate void ReportStat(int index,int wrote, int writing, double rate);

        public delegate void ProcessFunction(GenericLoader g,  int index, IRecordLoader r, ShapeUtilities.BaseRecord shape, int wrote);
        public delegate void ReportLength(GenericLoader g,  string dbffilename, string shpfilename, long length);
            
        public LoaderOptions Options { get; set; }

        /// <summary>
        /// This must contain a pointer to a method that creates the IRecordLoader class, this will be called during load
        /// </summary>
        public NewClass GetNewRecord;

        /// <summary>
        /// Fires when a record has been marked to be processed , intended for any custom code needed
        /// </summary>
        public event ProcessFunction ProcessRecord;

        /// <summary>
        /// Fires when a record has been skipped, intended to simply tell the implementor
        /// </summary>
        public event ProcessFunction SkipRecord;

        /// <summary>
        /// Fires when the present dbase files length is retrieved
        /// </summary>
        public event ReportLength OnLength;

        /// <summary>
        /// Fires when the thread reports back a status.
        /// </summary>
        public event ReportStat Status;
       

        public DataTable GetTable()
        {
            SqlConnection scon = new SqlConnection(Options.ConnectionString);
            scon.Open();

            DataTable dt = new DataTable();

            SqlCommand scom = new SqlCommand("select top 1 * from dbo."+ Options.TableName, scon);

            var r = scom.ExecuteReader();


            for (int x = 0; x < r.FieldCount; x++)
            {
                string name = r.GetName(x);

                if (!Options.FieldsToExclude.Contains( name))
                {
                    Type t = r.GetFieldType(x);
                    dt.Columns.Add(name, Options.FieldsToManuallyType.ContainsKey(name) ? Options.FieldsToManuallyType[name] : t);
                }
            }

            r.Close();

            scon.Close();

            return dt;
        }

        private List<IRecordLoader> towrite = new List<IRecordLoader>();
        private int wrote = 0;
        private double totalsecondswriting = 0;
        private double recordpersecond = 0;

        public void LoadZips()
        {
           
            Console.WriteLine("Processing Table " + Options.TableName);

            #region FilesAndDirectories
            var zipfiles = Directory.GetFiles(Options.FileDirectory, "*.zip");
            var outputdir = Options.FileDirectory + "\\" + Options.TempDirectoryName;            

            if (Directory.Exists(outputdir))
            {
                Directory.Delete(outputdir,true);
            }

            Directory.CreateDirectory(outputdir);
            #endregion FilesAndDirectories

            #region InitialSQL
            Console.WriteLine("Opening SQL Connection.");
            
            SqlConnection scon = new SqlConnection(Options.ConnectionString);
            scon.Open();

            if (Options.EmptyTable)
            {
                Console.WriteLine("Emptying table of records.");
                SqlCommand scom = new SqlCommand("truncate table dbo." + Options.TableName,scon);
                scom.ExecuteNonQuery();
            }


            List<object> resumeids = new List<object>();

            if (Options.Resume)
            {
                Console.WriteLine("Retrieving resume ids. Field " + Options.SqlResumeId + " selected.");
                SqlCommand getresumeids = new SqlCommand("select " + Options.SqlResumeId + " from dbo." + Options.TableName + " order by " + Options.SqlResumeId, scon);
                var ir = getresumeids.ExecuteReader();

                while (ir.Read()) { resumeids.Add(ir[Options.SqlResumeId]);  }

                ir.Close();
            }

            Console.WriteLine("Closing SQL Connection");
            scon.Close();

            #endregion InitialSQL

            #region ProcessZipFiles
            bool checkresume = resumeids.Count > 0;


            foreach (string z in zipfiles)
            {
                // extract zipfile contents
                ZipFile.ExtractToDirectory(z, outputdir);

                // retrieve shpfile and dbf file
                string dbasefile = Directory.GetFiles(outputdir, "*.dbf").First();
                string shpfile = Directory.GetFiles(outputdir, "*.shp").First();

                DbfDataReader.DbfDataReaderOptions ops = new DbfDataReaderOptions() { SkipDeletedRecords = true };
                DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(dbasefile,ops);
                
                ShapeUtilities.ShapeFile sfile = null;
         
                if (Options.LoadShapeFile)
                {
                    sfile = new ShapeUtilities.ShapeFile(shpfile);
                    sfile.Load();
                }

                int sindex = 0;

                // report length back to implementer.
                OnLength(this,dbasefile,shpfile, dr.DbfTable.Header.RecordCount);


                while (dr.Read())
                {
                    IRecordLoader i = GetNewRecord();
                    i.Read(dr);

                    // interesting... c# update anyone ?
                    ShapeUtilities.BaseRecord currshape = sfile?.Records[sindex].Record;
                    
                    if (checkresume)
                    {
                        if (resumeids.Contains(dr[Options.DbaseResumeId]))
                        {
                            resumeids.Remove(dr[Options.DbaseResumeId]);
                            checkresume = resumeids.Count > 0;
                            SkipRecord(this, sindex, i, currshape,wrote);
                            sindex++;
                            continue;
                        }
                    }

                    ProcessRecord(this, sindex, i, currshape,wrote);

                    towrite.Add(i);

                    Status(sindex, wrote, 0,  totalsecondswriting==0?0: wrote/totalsecondswriting);

                    if (Options.RecordLimit == towrite.Count)
                    {
                        Status(sindex, wrote, towrite.Count, recordpersecond);
                        DoCopy();
                        Status(sindex, wrote, 0, recordpersecond);
                        
                    }

                    sindex++;
                }

                dr.Close();

                if (towrite.Count>0)
                {
                    Status(sindex, wrote, towrite.Count, recordpersecond);
                    DoCopy();
                    Status(sindex, wrote, 0,recordpersecond);
                }

                Directory.Delete(outputdir,true);
            }

            #endregion ProcessZipFiles

        }

        private void DoCopy()
        {
            DateTime start = DateTime.Now;

            // if i want to error checking i'll just catch the exception in the calling block of code.
            DataTable dt = GetTable();
            
            foreach (IRecordLoader i in towrite)
            {
                i.PutRecord(dt);
            }

            SqlConnection scon = new SqlConnection(Options.ConnectionString);
            scon.Open();

            SqlBulkCopy sq = new SqlBulkCopy(scon);

            sq.DestinationTableName = Options.TableName;

            // doesnt this seem stupid ?
            // well i'd say yeah it was if they didnt design it stupidly to go off ordinal position rather than matching column names !
            foreach (DataColumn dc in dt.Columns)
            {
                sq.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
            }

            sq.WriteToServer(dt);

            scon.Close();

            wrote+=towrite.Count();


            towrite.Clear();

            DateTime end = DateTime.Now;

            totalsecondswriting += end.Subtract(start).TotalSeconds;
            recordpersecond = wrote / totalsecondswriting;
   
        }

    }

}
