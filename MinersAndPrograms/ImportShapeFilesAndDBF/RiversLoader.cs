using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CensusFiles;
using System.Threading;

namespace ImportShapeFilesAndDBF
{
    public class RiversLoader
    {
        public string ZipDirectory { get; set; }
        public bool EmptyRiversTable { get; set; }

        public string line;

        private long index;
        private SqlCommand insrec;
        private SqlConnection scon;
        private long length;
        private int x;
        private int y;
        public bool EventMode { get; set; }
        public bool Resume { get; set; }

        public RiversLoader(string zipdirectory, bool emptyRoadTable = false, bool eventmode = false, bool resume = false)
        {
            EventMode = eventmode;
            ZipDirectory = zipdirectory;
            EmptyRiversTable = emptyRoadTable;
            Resume = resume;

            for (int x = 0; x < 50; x++)
            {
                line += " ";
            }
        }

        public void LoadZips(string table = "Rivers")
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.IntegratedSecurity = true;
            scsb.DataSource = "localhost";
            scsb.InitialCatalog = "Geography";

            scon = new SqlConnection(scsb.ConnectionString);

            Console.WriteLine("Opening Sql Connection.");

            scon.Open();

            var zipfiles = Directory.GetFiles(ZipDirectory, "*.zip");
            var outputdir = ZipDirectory + "\\output\\";

            if (EmptyRiversTable)
            {
                Console.WriteLine("Emptying Rivers Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.Rivers", scon);
                trunccom.ExecuteNonQuery();
            }

            if (EventMode)
            {

                RiversRecord.OnParse += RiversRecord_OnParse;
                RiversRecord.OnFileLength += RiversRecord_OnFileLength;
                RiversRecord.SkipRecord += RiversRecord_SkipRecord;
            }


            foreach (string filename in zipfiles)
            {
                Console.WriteLine("Processing Archive: " + filename);

                if (Directory.Exists(outputdir))
                {
                    Directory.Delete(outputdir, true);
                }

                Directory.CreateDirectory(outputdir);

                ZipFile.ExtractToDirectory(filename, outputdir);

                string[] files = Directory.GetFiles(outputdir, "*.dbf");

                insrec = RiversRecord.GetInsert(scon);


                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.Write(line);
                index = 1;

                if (!EventMode)
                {
                    // load records and wkt strings.
                    List<RiversRecord> records = RiversRecord.ParseDBFFile(files[0], scon, true, false, false, Resume);

                    Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                    foreach (RiversRecord r in records)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.WriteLine("Processing Record " + index.ToString() + " of " + records.Count.ToString());

                        r.MapParameters(insrec);
                        insrec.ExecuteNonQuery();
                        index++;
                    }

                    Console.WriteLine();
                }
                else
                {
                    Task t = Task.Run(() =>
                    {
                        RiversRecord.ParseDBFFile(files[0], scon, true, false, true, Resume);
                    });

                    t.Wait();

                }

            }

            scon.Close();

            Console.WriteLine("Closed SQL Connection");


        }

        private void RiversRecord_SkipRecord(RiversRecord obj)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Skipping Record " + index.ToString() + " of " + length.ToString() + "          ");
            index++;
        }

        private void RiversRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        private const int limit = 500;
        private List<RiversRecord> queue = new List<RiversRecord>();
        private int wrote = 0;
        private void RiversRecord_OnParse(RiversRecord obj)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Processing Record " + index.ToString() + " of " + length.ToString());
            Console.WriteLine("Wrote " + wrote.ToString() + " to database thus far");

            queue.Add(obj);

            if (queue.Count == limit || index == length - 1)
            {

                
                Console.SetCursorPosition(x, y + 1);
                Console.WriteLine("Wrote: " + wrote.ToString() + "==>WRITING:"+queue.Count.ToString()+" <===                                ");

                // ---- second version
               // var insmult = RiversRecord.InsertMultiple(queue, scon);
               
                try
                {

                    // fourth fing revision now.
                    // leave john r sohns fucking work alone !
                    // third version is superior NOW THAT IT FUCKING WORKS.
                    // must install the dacfx package via nuget.
                    // ------ third version i did before -- problem is that i need to add fucking special components
                    // ------ because the sqlbulkcopy wants the udt geography to be the same between source and destination
                    // ------ believe that previously I went back to version 2 actually.
                    // ---- but we'll see as they continue to waste my fucking time.
                    DataTable dt = RiversRecord.GetTable(scon);
                    RiversRecord.FillTable(queue, dt);
                    SqlBulkCopy sb = RiversRecord.GetBulkCopier(dt, scon);
                    sb.WriteToServer(dt);
                    
                    wrote += queue.Count;

                    // ------------- second version -- only problem is not enough rows at once
                    //wrote+= insmult.ExecuteNonQuery();

                    // -------------first version -- only problem is too slow because of one at a time db writes
                    //      obj.MapParameters(insrec);
                    //        insrec.ExecuteNonQuery();
                }
                catch
                {
                    
                    scon.Close();
                    scon.Open();
                    Thread.Sleep(2000);

                                        
                    DataTable dt = RiversRecord.GetTable(scon);
                    RiversRecord.FillTable(queue, dt);
                    SqlBulkCopy sb = RiversRecord.GetBulkCopier(dt, scon);
                    sb.WriteToServer(dt);
                    wrote += queue.Count;

                    // ----- second version
                    //wrote+=insmult.ExecuteNonQuery();

                    // --- first version
                    //   insrec.ExecuteNonQuery();

                }

                queue.Clear();

                
            }

            Console.SetCursorPosition(x, y + 1);           
            Console.WriteLine("Wrote " + wrote.ToString() + " to database thus far                  ");

            index++;
        }
    }
}
