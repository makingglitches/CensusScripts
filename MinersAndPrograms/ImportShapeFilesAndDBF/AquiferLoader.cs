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
   public class AquiferLoader
    {
        public string ZipDirectory { get; set; }
        public bool EmptyAquiferTable { get; set; }

        public string line;

        private long index;
        private SqlCommand insrec;
        private SqlConnection scon;
        private long length;
        private int x;
        private int y;
        public bool EventMode { get; set; }

        public AquiferLoader(string zipdirectory, bool emptyAquiferTable = false, bool eventmode = false)
        {
            EventMode = eventmode;
            ZipDirectory = zipdirectory;
            EmptyAquiferTable = emptyAquiferTable;

            for (int x = 0; x < 50; x++)
            {
                line += " ";
            }
        }

        public void LoadZips(string table = "Aquifer")
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

            if (EmptyAquiferTable)
            {
                Console.WriteLine("Emptying Aquifer Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.Aquifer", scon);
                trunccom.ExecuteNonQuery();
            }

            if (EventMode)
            {
               
                AquiferRecord.OnParse += AquiferRecord_OnParse;
                AquiferRecord.OnFileLength += AquiferRecord_OnFileLength;
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

                insrec = AquiferRecord.GetInsert(scon);


                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.Write(line);
                index = 1;

                if (!EventMode)
                {
                    // load records and wkt strings.
                    List<AquiferRecord> records = AquiferRecord.ParseDBFFile(files[0], scon, true, false, false);

                    Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                    foreach (AquiferRecord r in records)
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
                        AquiferRecord.ParseDBFFile(files[0], scon, true, false, true);
                    });

                    t.Wait();

                }

            }

            scon.Close();

            Console.WriteLine("Closed SQL Connection");


        }

        private void AquiferRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        private void AquiferRecord_OnParse(AquiferRecord obj)
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
