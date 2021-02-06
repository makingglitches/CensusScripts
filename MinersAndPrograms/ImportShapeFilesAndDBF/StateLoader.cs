using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using CensusFiles;
using System.Threading;

namespace ImportShapeFilesAndDBF
{
   public class StateLoader
    {
        public string ZipDirectory { get; set; }
        public bool EmptyPlaceTable { get; set; }

        string line = "";

        public StateLoader(string zipdirectory, bool emptyPlaceTable, bool eventmode=false)
        {
            ZipDirectory = zipdirectory;
            EmptyPlaceTable = emptyPlaceTable;
            EventMode = eventmode;

            for (int x =0; x < 50; x++)
            {
                line += " ";
            }
        }

        public bool EventMode { get; set; }


        private long index;
        private SqlCommand insrec;
        private SqlConnection scon;
        private long length;
        private int x;
        private int y;

        public void LoadZips(string tablename="States")
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

            if (EmptyPlaceTable)
            {
                Console.WriteLine("Emptying Places Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.States", scon);
                trunccom.ExecuteNonQuery();
            }

            if (EventMode)
            {
                StateRecord.OnParse += StateRecord_OnParse;
                StateRecord.OnFileLength += StateRecord_OnFileLength;
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

                string [] files = Directory.GetFiles(outputdir, "*.dbf");

                // load records and wkt strings.

                index = 1;
                insrec = StateRecord.GetInsert(scon);

                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.Write(line);


                if (!EventMode)
                {
                    List<StateRecord> records = StateRecord.ParseDBFFile(files[0], scon, true,false,false);
                    Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                    foreach (StateRecord r in records)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.WriteLine("Processing Record " + index.ToString() + " of " + records.Count.ToString());
                        if (!EventMode)
                            r.MapParameters(insrec);
                        insrec.ExecuteNonQuery();
                        index++;
                    }
                }
                else
                {

                    Task t = Task.Run(() =>
                    {
                        StateRecord.ParseDBFFile(files[0], scon, true, false, true);
                    });

                    t.Wait();

                }

                Console.WriteLine();
            }

            scon.Close();

            Console.WriteLine("Closed SQL Connection");

        }

        private void StateRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        private void StateRecord_OnParse(StateRecord obj)
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
                Thread.Sleep(10000);
                insrec.ExecuteNonQuery();

            }

            index++;
        }
    }
}
