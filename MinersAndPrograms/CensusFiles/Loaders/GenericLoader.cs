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
    public abstract class GenericLoader
    {

        /// <summary>
        /// Delegate representing the thingamajig that creates new class instances.
        /// </summary>
        /// <returns></returns>
        public delegate IRecordLoader NewClass();
        public delegate void ReportStat(int index, int wrote, int writing);

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

        public void LoadZips(LoaderOptions options)
        {
            Options = options;

            Console.WriteLine("Processing Table " + options.TableName);

            #region FilesAndDirectories
            var zipfiles = Directory.GetFiles(options.FileDirectory, "*.zip");
            var outputdir = options.FileDirectory + "\\" + options.TempDirectoryName;            

            if (Directory.Exists(outputdir))
            {
                Directory.Delete(outputdir);
            }

            Directory.CreateDirectory(outputdir);
            #endregion FilesAndDirectories

            #region InitialSQL
            Console.WriteLine("Opening SQL Connection.");
            
            SqlConnection scon = new SqlConnection(options.ConnectionString);
            scon.Open();

            if (options.EmptyTable)
            {
                Console.WriteLine("Emptying table of records.");
                SqlCommand scom = new SqlCommand("truncate table dbo." + options.TableName,scon);
                scom.ExecuteNonQuery();
            }


            List<object> resumeids = new List<object>();

            if (options.Resume)
            {
                Console.WriteLine("Retrieving resume ids. Field " + options.SqlResumeId + " selected.");
                SqlCommand getresumeids = new SqlCommand("select " + options.SqlResumeId + " from dbo." + options.TableName + " order by " + options.SqlResumeId, scon);
                var ir = getresumeids.ExecuteReader();

                while (ir.Read()) { resumeids.Add(ir[options.SqlResumeId]);  }

                ir.Close();
            }

            Console.WriteLine("Closing SQL Connection");
            scon.Close();

            #endregion InitialSQL

            bool checkresume = resumeids.Count > 0;

            int wrote = 0;

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
         
                if (options.LoadShapeFile)
                {
                    sfile = new ShapeUtilities.ShapeFile(shpfile);
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
                        if (resumeids.Contains(dr[options.DbaseResumeId]))
                        {
                            resumeids.Remove(dr[options.DbaseResumeId]);
                            checkresume = resumeids.Count > 0;
                            SkipRecord(this, sindex, i, currshape,wrote);
                            sindex++;
                            continue;
                        }
                    }

                    ProcessRecord(this, sindex, i, currshape,wrote);

                    towrite.Add(i);

                    if (Options.RecordLimit == towrite.Count)
                    {

                    }

                    sindex++;
                }

                dr.Close();


                Directory.Delete(outputdir);
            }

            

        }

    }

}
