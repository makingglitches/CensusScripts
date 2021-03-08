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
                this.ProcessRecord += GenericLoader_ProcessRecord;
            }
        }

        public GetDerivedKey DerivedKeyGenerator { get; set; }

        private void GenericLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            // catchall to avoid unnecessary processrecord event handlers in implementations.
            // this should really just be all of them really.
            // however fips calculation may not allow this in a few record types.
            // ... and just in case there is a usecase where we're loading a state supplied dbf WITHOUT a shapefile.
            if (r is IHasShape)
            {
                IHasShape rec = (IHasShape)r;
                rec.Shape = shape;
            }
        }

        private void GenericLoader_ReportFinalStats(int wrote, int skipped, double totalseconds, double recordswrotepersecond)
        {
            Console.WriteLine("Total Run Time: " + TimeSpan.FromSeconds(totalseconds).ToString());
            Console.WriteLine("Processed " + wrote.ToString() + " records.");
            Console.WriteLine("Skipped " + skipped.ToString() + " records.");
            Console.WriteLine("Avg Rate " + recordwrotepersecond.ToString() + " records/second");
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

        private void GenericLoader_SkipRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine(blankline);
            Console.WriteLine(blankline);
            Console.SetCursorPosition(cursorx, cursory);
            Console.WriteLine("Skipping " + (index+1).ToString() + " of " + dbflength.ToString());
        }

        #endregion ConsoleLogging EventHandlers

        #region Event Delegates

        public delegate object GetDerivedKey(DbfDataReader.DbfDataReader d, GenericLoader g);

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
        public delegate void ProcessFunction(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape);

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

        private int retries = 0;
        private double totalsecondsprocessing = 0;
        private double totalsecondswriting = 0;
        private double recordwrotepersecond = 0;
        private double recordsprocpersecond = 0;
        private int skippedrecords = 0;
        private string currentDBFName;
        private string currentSHPName;
        private int sindex = 0;
        #endregion Private Processing Fields

        #region Current FileNames
        public string DBFFileName { get { return currentDBFName; } }
        public string SHPFileName { get { return currentSHPName; } }
        #endregion Current FilesNames

        #region Public Batch Fields

        public double BatchSecondsProcessing = 0;
        public double BatchSecondsWriting = 0;
        public  int BatchRecordsWrote = 0;
        public int BatchRetries = 0;
        public int BatchRecordsSkipped = 0;
        public double BatchRecordsWrotePerSecond = 0;
        public double BatchRecordsProcsPerSecond = 0;
        #endregion Public Batch Fields

        public void LoadZips()
        {

            string summaryfilename = Options.TableName + " Load Summary.txt";

            if (Options.WriteSummaryFile)
            {
                // filename
                // written records
                // skipped records
                // time processing
                // time writing to server
                // records processed / s
                // records written / s
                File.WriteAllText(summaryfilename,
                    "Input File\tRecord Count\tWrote Records\tSkipped Existing\tTime Processing\tTime Writing To Server\tRetries\tRecords Proc/s\tRecords Wrote/s\n");
            }

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

            if (Options.Resume)
            {

                DateTime startcrunch = DateTime.Now;

                Dictionary<string, List<object>> files = new Dictionary<string, List<object>>(); 

                foreach (string z in zipfiles)
                {
                    ZipArchive za = ZipFile.OpenRead(z);

                    foreach (ZipArchiveEntry ze in za.Entries)
                    {
                        if (ze.Name.EndsWith(".dbf"))
                        {
                            ze.ExtractToFile("local.dbf", true);
                            
                            DbfDataReader.DbfDataReader d = 
                                new DbfDataReader.DbfDataReader("local.dbf", new DbfDataReaderOptions() { SkipDeletedRecords = true });

                            List<object> ids = new List<object>();

                            while (d.Read())
                            {
                                if (Options.DerivedResumeKey)
                                {
                                    ids.Add(DerivedKeyGenerator(d, this));
                                }
                                else
                                {
                                    ids.Add(d[Options.DbaseResumeId]);
                                }
                            }

                            // add the files ids into the dictionary for later comparison against the database
                            // will transmit ALL ids to the database per file and see if there are 
                            files.Add(ze.FullName, ids);


                        }
                    }
                }


                DateTime endcrunch = DateTime.Now;
            }

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
                currentDBFName  = Directory.GetFiles(outputdir, "*.dbf").First();
                currentSHPName = Directory.GetFiles(outputdir, "*.shp").First();

                DbfDataReader.DbfDataReaderOptions ops = new DbfDataReaderOptions() { SkipDeletedRecords = true };
                DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(currentDBFName, ops);

                ShapeUtilities.ShapeFile sfile = null;

                if (Options.LoadShapeFile)
                {
                    if (Options.ConsoleLogging) Console.WriteLine("Loading shapefile " +  Path.GetFileName(currentSHPName));

                    sfile = new ShapeUtilities.ShapeFile(currentSHPName);
                    sfile.Load();
                }

                sindex = 0;

                // report length back to implementer.
                OnLength(this, currentDBFName, currentSHPName, dr.DbfTable.Header.RecordCount);


                while (dr.Read())
                {
                    DateTime startproc = DateTime.Now;
                    DateTime endproc;

                    IRecordLoader i = GetNewRecord();
                    i.Read(dr);

                    // interesting... c# update anyone ?
                    ShapeUtilities.BaseShapeRecord currshape = sfile?.Records[sindex].Record;

                    if (checkresume)
                    {

                        object key = Options.DerivedResumeKey ? DerivedKeyGenerator(dr, this) : dr[Options.DbaseResumeId];
                  
                        if (resumeids.Contains(key))
                        {
                            resumeids.Remove(dr[Options.DbaseResumeId]);
                            checkresume = resumeids.Count > 0;
                            SkipRecord(this, sindex, i, currshape);
                            skippedrecords++;
                            sindex++;
                            endproc = DateTime.Now;
                            totalsecondsprocessing += startproc.Subtract(endproc).TotalSeconds;

                            continue;
                        }
                    }

                    // allow user specfied code to run which performs actions on 
                    // the loaded record 
                    ProcessRecord(this, sindex, i, currshape);
                    endproc = DateTime.Now;

                    totalsecondsprocessing += startproc.Subtract(endproc).TotalSeconds;
                    
                    towrite.Add(i);

                    Status(sindex, wrote, 0, totalsecondswriting == 0 ? 0 : wrote / totalsecondswriting);

                    if (Options.RecordLimit == towrite.Count)
                    {
                        DoTableWrite();
                    }


                    sindex++;
                }

                dr.Close();

                if (towrite.Count > 0)
                {
                    DoTableWrite();
                }

                Directory.Delete(outputdir, true);

                recordsprocpersecond = (wrote + skippedrecords) / totalsecondsprocessing;

                if (Options.WriteSummaryFile)
                {
                    // filename
                    // written records
                    // skipped records
                    // time processing
                    // time writing to server
                    // records processed / s
                    // records written / s
                    File.AppendAllText(summaryfilename,
                        Path.GetFileName(z) + "\t" +
                        wrote.ToString() + "\t" +
                        skippedrecords.ToString() + "\t" +
                        TimeSpan.FromSeconds(totalsecondsprocessing).ToString() + "\t" +
                        TimeSpan.FromSeconds(totalsecondswriting).ToString() + "\t" +
                        recordsprocpersecond.ToString() + "\t"+
                        recordwrotepersecond.ToString() + "\t\n");


                }


                ReportFinalStats(wrote, skippedrecords, totalsecondswriting, recordwrotepersecond);

                BatchRecordsSkipped += skippedrecords;
                BatchRecordsWrote += wrote;
                BatchRetries += retries;
                BatchSecondsProcessing += totalsecondsprocessing;
                BatchSecondsWriting += totalsecondswriting;
              

                retries = 0;
                totalsecondsprocessing = 0;
                wrote = 0;
                totalsecondswriting = 0;
                skippedrecords = 0;
                recordwrotepersecond = 0;
            }

            #endregion ProcessZipFiles

            BatchRecordsWrotePerSecond = BatchRecordsWrote / BatchSecondsWriting;
            BatchRecordsProcsPerSecond = (BatchRecordsSkipped + BatchRecordsWrote) / BatchSecondsProcessing;




            if (Options.WriteSummaryFile)
            {
                // filename
                // written records
                // skipped records
                // time processing
                // time writing to server
                // records processed / s
                // records written / s
                File.AppendAllText(summaryfilename,
                    "Batch Totals\t" +
                    BatchRecordsWrote.ToString() + "\t" +
                    BatchRecordsSkipped.ToString() + "\t" +
                    TimeSpan.FromSeconds(BatchSecondsProcessing).ToString() + "\t" +
                    TimeSpan.FromSeconds(BatchSecondsWriting).ToString() + "\t" +
                    recordsprocpersecond.ToString() + "\t" +
                    recordwrotepersecond.ToString() + "\n");
            }

            // let senoir chomo cripple get fucked up some more, getting a little sick of seeing people stare at children.
            // fuck them for now.
            // scon =  Options.MakeConnection();

            scon.Open();
            SqlCommand getcount = new SqlCommand("select count(*) from dbo." + Options.TableName, scon);
         
            int sqlrecords = getcount.ExecuteNonQuery();

            if ( sqlrecords == BatchRecordsSkipped+BatchRecordsWrote)
            {
                Console.WriteLine("Record number in table matches progress thus far. A total of " + sqlrecords.ToString() + " discovered.");
            }
            else
            {
                Console.WriteLine("Differing counts");
                Console.WriteLine("Server returned count: " + sqlrecords.ToString());
                Console.WriteLine("Skipped + Processed: " + (BatchRecordsWrote + BatchRecordsSkipped).ToString());
            }
           
            scon.Close();

            // whats going to be really funny is when i bury all snarky messages
            // by changing single lines
            // or deleting and burying these comments so github cant pretend its not complicit in burying things.
            // hey heres an idea, how about you people tap the government that is intentionally delaying the world from being a better place
            // and enabling chomo fags like all of you, to in some way improve the way it monetizes public services like cloud storage
            // for honest citizens and keep these fags from doing business ? by say taking services that were rebooted 
            // and swapping photos out etc.
        }

        #region Sql Server Functions

        private void DoTableWrite()
        {
            int localretries = 0;

            Status(sindex, wrote, towrite.Count, recordwrotepersecond);

            while (localretries < Options.Retries)
            {
                try
                {
                    DoCopy();
                    break;
                }
                catch (Exception e)
                {
                    localretries++;

                    if (localretries == Options.Retries)
                    {
                        Console.WriteLine("Maximum number of retries reached. Exiting.");
                        File.WriteAllText("error.txt", e.ToString());
                        Console.WriteLine("See error.txt for exception information.");
                        return;
                    }

                    Console.WriteLine("Waiting " + Options.SecondsToWait.ToString());
                    System.Threading.Thread.Sleep(Options.SecondsToWait * 1000);
                }
            }

            retries += localretries;

            Status(sindex, wrote, 0, recordwrotepersecond);

        }

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

        // these people are so fucked up some of them like fucking and abusing and being abused by crippled and retarded people.
        // fuck that.
        // interesting fact, statistically, cripples, deformed people, midgets and extremely ugly morbidly obese people are at a high risk
        // of being child molesters, under NATURAL not intentional circumstances.
        // its ugly truths like this they try to keep us from knowing, like that most sex offenders that target children and babies tend to 
        // be women. most especially those who grow up being prostituted or with a history of sexual abuse in general.
        // nope, they want weaponized laws that make us blind to possibilities.
        // heres an ugly one, some people who are crippled in their insane asylum society, crippled children and traded their health and limb to do so.

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

            //.... sighhhhhhhhhhhh 
            sq.WriteToServer(dt);

            scon.Close();

            wrote += towrite.Count();

            towrite.Clear();

            DateTime end = DateTime.Now;

            totalsecondswriting += end.Subtract(start).TotalSeconds;
            recordwrotepersecond = wrote / totalsecondswriting;

        }

        #endregion Sql Server Functions

    }

}
