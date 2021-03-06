﻿using System;
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
        #region Initialization

        public GenericLoader(LoaderOptions options)
        {
            Options = options;

            if (options.ConsoleLogging)
            {
                this.ReportFinalStats += GenericLoader_ReportFinalStats;
                this.SkipRecord += GenericLoader_SkipRecord;
                this.Status += GenericLoader_Status;
                this.OnLength += GenericLoader_OnLength;
            }
        }

        private void GenericLoader_ReportFinalStats(int wrote, int skipped, double totalseconds, double recordswrotepersecond)
        {
            Console.WriteLine("Total Run Time: " + TimeSpan.FromSeconds(totalseconds).ToString());
            Console.WriteLine("Processed " + wrote.ToString() + " records.");
            Console.WriteLine("Skipped " + wrote.ToString() + " records.");
            Console.WriteLine("Avg Rate " + recordpersecond.ToString() + " records/second");
        }

        public LoaderOptions Options { get; set; }

        /// <summary>
        /// Delegate representing the thingamajigger that creates new class instances for the record class being loaded.
        /// </summary>
        /// <returns></returns>
        public delegate IRecordLoader NewClass();

        /// <summary>
        /// This must contain a pointer to a method that creates the IRecordLoader class, this will be called during load
        /// </summary>
        /// <remarks>
        /// Needs to be assigned an anonymous or real factory method that creates an IRecordWriter derivative.
        /// </remarks>
        public NewClass GetNewRecord { get; set; }

        #endregion Initialization

        #region ConsoleLogging EventHandlers

        private long dbflength = 0;
        private int cursorx = 0;
        private int cursory = 0;
        private string blankline = "";

        private void GenericLoader_OnLength(GenericLoader g, string dbffilename, string shpfilename, long length)
        {
            dbflength = length;

            Console.WriteLine("Processing Dbase: " + Path.GetFileName(dbffilename));
            Console.WriteLine("Processing shp: " + Path.GetFileName(shpfilename));
            Console.WriteLine("Discovered " + length.ToString() + " Records.");

            if (g.Options.Resume)
            {
                Console.WriteLine("===> Resume On <===");
            }



            // interesting tidbit Console.LargestWindowWidth doesnt work worth shit. 
            cursorx = Console.CursorLeft;
            cursory = Console.CursorTop;

            blankline = string.Empty;

            for (int rep = 0; rep < 80; rep++)
            {
                blankline += " ";
            }
        }

        private void GenericLoader_Status(int index, int wrote, int writing, double rate)
        {
            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine(blankline);
            Console.WriteLine(blankline);

            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine("Processing record " + (index + 1).ToString() + " of " + dbflength.ToString());
            Console.WriteLine((writing > 0 ? "===>WRITING:" + writing.ToString() + " <====" : "") + "Wrote: " + wrote.ToString() + " to Database @ " + rate.ToString() + " r/s");

        }

        private void GenericLoader_SkipRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseRecord shape)
        {
            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine(blankline);
            Console.WriteLine(blankline);
            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine("Skipping " + index.ToString() + " of " + dbflength.ToString());
        }

        #endregion ConsoleLogging EventHandlers

        #region Event Delegates

        /// <summary>
        /// Represents an event handler which reports back stats about all processing
        /// </summary>
        /// <param name="wrote"></param>
        /// <param name="skipped"></param>
        /// <param name="totalseconds"></param>
        /// <param name="recordswrotepersecond"></param>
        public delegate void FinalStats(int wrote, int skipped, double totalseconds, double recordswrotepersecond);


        /// <summary>
        /// Represents an event handler which reports back stats about progress
        /// </summary>
        /// <param name="index"></param>
        /// <param name="wrote"></param>
        /// <param name="writing"></param>
        /// <param name="rate"></param>
        public delegate void ReportStat(int index, int wrote, int writing, double rate);

        /// <summary>
        /// Represents a call which indicates one of the two outcomes of record load which is skip or process
        /// </summary>
        /// <param name="g"></param>
        /// <param name="index"></param>
        /// <param name="r"></param>
        /// <param name="shape"></param>
        /// <param name="wrote"></param>
        public delegate void ProcessFunction(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseRecord shape);

        /// <summary>
        /// Reports the initial length and other file information to the event handler
        /// </summary>
        /// <param name="g"></param>
        /// <param name="dbffilename"></param>
        /// <param name="shpfilename"></param>
        /// <param name="length"></param>
        public delegate void ReportLength(GenericLoader g, string dbffilename, string shpfilename, long length);

        #endregion Event Delegates

        #region Events
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

        /// <summary>
        /// Fires at the end of each zipfile being processed
        /// </summary>
        public event FinalStats ReportFinalStats;
        #endregion Events

        #region Private Processing Fields
        private List<IRecordLoader> towrite = new List<IRecordLoader>();
        private int wrote = 0;
        private double totalsecondswriting = 0;
        private double recordpersecond = 0;
        private int skippedrecords = 0;
        #endregion Private Processing Fields

        public void LoadZips()
        {
            if (Options.ConsoleLogging)
            {
                Console.WriteLine("Processing Table " + Options.TableName);
            }

            #region FilesAndDirectories
            var zipfiles = Directory.GetFiles(Options.FileDirectory, "*.zip");
            var outputdir = Options.FileDirectory + "\\" + Options.TempDirectoryName;

            if (Directory.Exists(outputdir))
            {
                Directory.Delete(outputdir, true);
            }

            Directory.CreateDirectory(outputdir);
            #endregion FilesAndDirectories

            #region InitialSQL
            // Console.WriteLine("Opening SQL Connection.");

            SqlConnection scon = new SqlConnection(Options.ConnectionString);
            scon.Open();

            if (Options.EmptyTable)
            {
                if (Options.ConsoleLogging) Console.WriteLine("Emptying table of records.");
                SqlCommand scom = new SqlCommand("truncate table dbo." + Options.TableName, scon);
                scom.ExecuteNonQuery();
            }


            List<object> resumeids = new List<object>();

            if (Options.Resume)
            {
                if (Options.ConsoleLogging) Console.WriteLine("Retrieving resume ids. Field " + Options.SqlResumeId + " selected.");
                SqlCommand getresumeids = new SqlCommand("select " + Options.SqlResumeId + " from dbo." + Options.TableName + " order by " + Options.SqlResumeId, scon);
                var ir = getresumeids.ExecuteReader();

                while (ir.Read()) { resumeids.Add(ir[Options.SqlResumeId]); }

                ir.Close();
            }

            //Console.WriteLine("Closing SQL Connection");

            scon.Close();

            #endregion InitialSQL

            #region ProcessZipFiles
            bool checkresume = resumeids.Count > 0;

            // wanna make me happy stop raping and selling kids
            // and dont make me think things like all women especially workers are fucking chomos

            foreach (string z in zipfiles)
            {
                if (Options.ConsoleLogging) Console.WriteLine("Extracting contents of archive " + Path.GetFileName(z));

                // extract zipfile contents
                ZipFile.ExtractToDirectory(z, outputdir);

                // retrieve shpfile and dbf file
                string dbasefile = Directory.GetFiles(outputdir, "*.dbf").First();
                string shpfile = Directory.GetFiles(outputdir, "*.shp").First();

                DbfDataReader.DbfDataReaderOptions ops = new DbfDataReaderOptions() { SkipDeletedRecords = true };
                DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(dbasefile, ops);

                ShapeUtilities.ShapeFile sfile = null;

                if (Options.LoadShapeFile)
                {
                    if (Options.ConsoleLogging) Console.WriteLine("Loading shapefile " + shpfile);

                    sfile = new ShapeUtilities.ShapeFile(shpfile);
                    sfile.Load();
                }

                int sindex = 0;

                // report length back to implementer.
                OnLength(this, dbasefile, shpfile, dr.DbfTable.Header.RecordCount);


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
                            SkipRecord(this, sindex, i, currshape);
                            skippedrecords++;
                            sindex++;
                            continue;
                        }
                    }

                    // allow user specfied code to run which performs actions on 
                    // the loaded record 
                    ProcessRecord(this, sindex, i, currshape);

                    towrite.Add(i);

                    Status(sindex, wrote, 0, totalsecondswriting == 0 ? 0 : wrote / totalsecondswriting);

                    if (Options.RecordLimit == towrite.Count)
                    {
                        Status(sindex, wrote, towrite.Count, recordpersecond);
                        DoCopy();
                        Status(sindex, wrote, 0, recordpersecond);

                    }

                    sindex++;
                }

                dr.Close();

                if (towrite.Count > 0)
                {
                    Status(sindex, wrote, towrite.Count, recordpersecond);
                    DoCopy();
                    Status(sindex, wrote, 0, recordpersecond);
                }

                Directory.Delete(outputdir, true);

                ReportFinalStats(wrote, skippedrecords, totalsecondswriting, recordpersecond);
            }

            #endregion ProcessZipFiles
        }

        #region Sql Server Functions
        public DataTable GetTable()
        {
            SqlConnection scon = new SqlConnection(Options.ConnectionString);
            scon.Open();

            DataTable dt = new DataTable();

            SqlCommand scom = new SqlCommand("select top 1 * from dbo." + Options.TableName, scon);

            var r = scom.ExecuteReader();


            for (int x = 0; x < r.FieldCount; x++)
            {
                string name = r.GetName(x);

                if (!Options.FieldsToExclude.Contains(name))
                {
                    Type t = r.GetFieldType(x);
                    dt.Columns.Add(name, Options.FieldsToManuallyType.ContainsKey(name) ? Options.FieldsToManuallyType[name] : t);
                }
            }

            r.Close();

            scon.Close();

            return dt;
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

            wrote += towrite.Count();

            towrite.Clear();

            // hey this is tori and zimmeran talking btw, I wouldnt have thought any of this
            // I was quite confused even after years and had totally different and relatively normal perceptions
            // of some of their weird ass behavior that aligned with an ordinary mind created by standard western civilization
            // not this weird disgusting bullshit that pretty much eliminates all reason to do much of anything.
            // still btw. as in second time i wrote this comment.
            DateTime end = DateTime.Now;

            totalsecondswriting += end.Subtract(start).TotalSeconds;
            recordpersecond = wrote / totalsecondswriting;

        }

        #endregion Sql Server Functions

    }

}
