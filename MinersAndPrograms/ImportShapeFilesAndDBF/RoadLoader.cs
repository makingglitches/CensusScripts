using System;
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
   public class RoadLoader
    {
       // oh yeah not the first time they wasted my time at all !
       // god knows what the rest of the world sees but i see myself 
       // in a place where they dont let the record of thinsg change much and are doing their best to cut nuances of 
       // crap they can't explain with their perverted retard code
       // and i'm pretty certain its about 2031. not 2021... though not certain.
        public string ZipDirectory { get; set; }
        public bool EmptyRoadTable { get; set; }

        public string line;

        private long index;
        private SqlCommand insrec;
        private SqlConnection scon;
        private long length;
        private int x;
        private int y;
        public bool EventMode { get; set; }

        public RoadLoader(string zipdirectory, bool emptyRoadTable=false, bool eventmode=false)
        {
            EventMode = eventmode;
            ZipDirectory = zipdirectory;
            EmptyRoadTable = emptyRoadTable;

            for (int x = 0; x < 50; x++)
            {
                line += " ";
            }
        }

        public void LoadZips(string table="Roads")
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

            if (EmptyRoadTable)
            {
                Console.WriteLine("Emptying Roads Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.Roads", scon);
                trunccom.ExecuteNonQuery();
            }

            if (EventMode)
            {
                RoadRecord.OnParse += RoadRecord_OnParse;
                RoadRecord.OnFileLength += RoadRecord_OnFileLength;
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

                insrec = RoadRecord.GetInsert(scon);

         
                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.Write(line);
                index = 1;

                if (!EventMode)
                {
                    // load records and wkt strings.
                    List<RoadRecord> records = RoadRecord.ParseDBFFile(files[0], scon, true,false,false);

                    Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                    foreach (RoadRecord r in records)
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
                        RoadRecord.ParseDBFFile(files[0], scon, true, false, true);
                    });

                    t.Wait();

                }

            }

            scon.Close();

            Console.WriteLine("Closed SQL Connection");


        }

        private void RoadRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        private void RoadRecord_OnParse(RoadRecord obj)
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
