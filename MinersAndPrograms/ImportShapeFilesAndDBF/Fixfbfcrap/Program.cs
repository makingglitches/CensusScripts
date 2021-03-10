using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.IO;
using ShapeUtilities;
using CensusFiles;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Data;
using CensusFiles.Loaders;
using CensusFiles.Utilities;
using System.IO.Compression;

namespace Fixfbfcrap
{
    class Program
    {

        // god i love source control
        
        static void Main(string[] args)
        {

            string locspecdir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\SpeciesData";


            SqlConnection scon = new SqlConnection();

            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder()
            {
                InitialCatalog ="Geography",
                IntegratedSecurity=true
            };

            scon.ConnectionString = scsb.ConnectionString;

            scon.Open();

            SqlCommand scom = new SqlCommand("select ArchiveName from dbo.Species", scon);

            var dr = scom.ExecuteReader();

            List<string> files = Directory.GetFiles(locspecdir).Select(o=>Path.GetFileName(o)).ToList();
            List<string> missing = new List<string>();

            while (dr.Read())
            {
                
                if (!files.Contains(dr["ArchiveName"].ToString()))
                {
                    missing.Add(dr["ArchiveName"].ToString());
                }
                
            }

            // they are rather sick in that when they destroy a persons personal work and hide photos
            // they took of themselves when they are younger they are encouraging themselves to be 
            // murdered as they should be.
            // by what excuse do people steal years of someones life, photos, changes, writing, etc 
            // and then claim it never happened and in the long run fuck themselves as well ?
            dr.Close();
            scon.Close();


            //string locspecdir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\SpeciesData";

            //SpeciesRepackager.Repackage(locspecdir, locspecdir + "\\repack",null);

            //string[] files = Directory.GetFiles(locspecdir, "*.zip");

            //List<string> containedzip = new List<string>();

            //bool end = false;

            //foreach (string f in files)
            //{
            //    ZipArchive za = ZipFile.OpenRead(f);

            //    foreach (ZipArchiveEntry s in za.Entries)
            //    {
            //        if (s.Name.ToLower().EndsWith(".zip"))
            //        {
            //            if (containedzip.Contains(s.Name.ToLower()))
            //            {
            //                end = true;
            //                Console.WriteLine("Found duplicate subarchive name");
            //                break ;
            //            }
            //            else
            //            {
            //                containedzip.Add(s.Name.ToLower());
            //            }
            //        }
            //    }

            //    if (end)
            //    {
            //        break;
            //    }

            //}

            //if (!end) Console.WriteLine("No Duplicates found.");

            //Console.ReadLine();
        }
       
    }
}
