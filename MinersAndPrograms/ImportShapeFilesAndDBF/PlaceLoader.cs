using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using CensusFiles;

namespace ImportShapeFilesAndDBF
{
   public class PlaceLoader
    {
        public string ZipDirectory { get; set; }
        public bool EmptyPlaceTable { get; set; }

        string line = "";

        public PlaceLoader(string zipdirectory, bool emptyPlaceTable)
        {
            ZipDirectory = zipdirectory;
            EmptyPlaceTable = emptyPlaceTable;

            for (int x =0; x < 50; x++)
            {
                line += " ";
            }
        }

        public void LoadZips(string tablename="Places")
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.IntegratedSecurity = true;
            scsb.DataSource = "localhost";
            scsb.InitialCatalog = "Geography";

            SqlConnection scon = new SqlConnection(scsb.ConnectionString);

            Console.WriteLine("Opening Sql Connection.");

            scon.Open();

            var zipfiles = Directory.GetFiles(ZipDirectory, "*.zip");
            var outputdir = ZipDirectory + "\\output\\";

            if (EmptyPlaceTable)
            {
                Console.WriteLine("Emptying Places Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.Places", scon);
                trunccom.ExecuteNonQuery();
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
                List<PlaceRecord> records = PlaceRecord.ParseDBFFile(files[0], scon, true);

                SqlCommand insrec = PlaceRecord.GetInsert(scon);

                Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                int x = Console.CursorLeft;
                int y = Console.CursorTop;

                Console.Write(line);
                int index = 1;

                foreach (PlaceRecord r in records)
                {
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine("Processing Record "+index.ToString() + " of " + records.Count.ToString());

                    r.MapParameters(insrec);
                    insrec.ExecuteNonQuery();
                    index++;
                }

                Console.WriteLine();
            }
            scon.Close();

            Console.WriteLine("Closed SQL Connection");

        }
    }
}
