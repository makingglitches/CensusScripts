﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        public RiversLoader(string zipdirectory, bool emptyRoadTable = false, bool eventmode = false, bool resume=false)
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
                    List<RiversRecord> records = RiversRecord.ParseDBFFile(files[0], scon, true, false, false,Resume);

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
                        RiversRecord.ParseDBFFile(files[0], scon, true, false, true,Resume);
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
            Console.WriteLine("Skipping Record " + index.ToString() + " of " + length.ToString()+"          ");
            index++;
        }

        private void RiversRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        private void RiversRecord_OnParse(RiversRecord obj)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Processing Record " + index.ToString() + " of " + length.ToString());

            try
            {
                obj.MapParameters(insrec);
                insrec.ExecuteNonQuery();
            }
            catch
            {
                scon.Close();
                scon.Open();
                Thread.Sleep(2000);
                insrec.ExecuteNonQuery();

            }

            index++;
        }
    }
}
