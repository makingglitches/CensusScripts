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



namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {
            var dread = new DbfDataReader.DbfDataReader(@"C:\Users\John\Documents\CensusProject\CensusShapeFileData\StateZips\tl_2019_us_state\tl_2019_us_state.dbf");


            ClassGenerator.WriteClassBase("StateBase", "StateBase.cs", dread);

            //var dread = new DbfDataReader.DbfDataReader(@"C:\Users\John\Documents\CensusProject\CensusShapeFileData\CountyZips\tl_2019_us_county\tl_2019_us_county.dbf");


            //ClassGenerator.WriteClassBase("CountyBase", "CountyBase.cs", dread);

            // string shpfiledir = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Places";
            // string sampleshpfile = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Places\tl_2019_01_place.shp";

            //var shpfiles =  Directory.GetFiles(shpfiledir, "*.shp");

            // foreach (string filename in shpfiles)
            // {
            //     ShapeFile s = new ShapeFile(filename);
            //     s.Load();

            //     int nonpoly = 0;

            //     foreach (var r in s.Records)
            //     {
            //         if (!(r.Record is PolygonShape))
            //         {
            //             nonpoly++;
            //         }
            //     }

            //     if(nonpoly >0)
            //     {
            //         Console.WriteLine(filename + "  contains " + nonpoly.ToString() + " records.");
            //     }
            // }


            // string roaddir = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Roads";
            // string roaddbf = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Roads\tl_2019_01001_roads.dbf";
            // string roadshp = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Roads\tl_2019_01001_roads.shp";

            // SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder();
            // scb.InitialCatalog = "Geography";
            // scb.IntegratedSecurity = true;

            // SqlConnection scon = new SqlConnection(scb.ConnectionString);
            // scon.Open();

            // var roaddbfrecords = RoadRecord.ParseDBFFile(roaddbf, scon,true);

            // ShapeFile road = new ShapeFile(roadshp);
            // road.Load();

            //Console.WriteLine( road.Records[0].Record.GetWKT());


            // DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(roaddbf);

            //ClassGenerator.WriteClassBase("RoadBase", "RoadBase.cs", dread);

            //  Console.WriteLine(s.Records[0].Record.GetWKT());

            //SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder();
            //scb.InitialCatalog = "Geography";
            //scb.IntegratedSecurity = true;

            //SqlConnection scon = new SqlConnection(scb.ConnectionString);
            //scon.Open();

            //string sampledbffile = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Places\tl_2019_01_place.dbf";

            //var places = PlaceRecord.ParseDBFFile(sampledbffile,scon);



            //SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder();
            //scb.InitialCatalog = "Geography";
            //scb.IntegratedSecurity = true;

            //SqlConnection scon = new SqlConnection(scb.ConnectionString);
            //scon.Open();

            //SqlCommand sq = new SqlCommand("select top 100 * from dbo.FipsKeys", scon);

            //SqlDataReader sr = sq.ExecuteReader();

            //ClassGenerator.WriteClassBase("FipsKeyBase", "FipsKeyBase.cs", sr);

            //sr.Close();

            //scon.Close();



            //string sampledbffile = @"C:\Users\John\Documents\QrCode\Input\Places2019\Places\tl_2019_01_place.dbf";

            //DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(sampledbffile);

            //PlaceRecord pr = new PlaceRecord();
            //dr.Read();
            //pr.Read(dr);



            //CensusFiles.ClassGenerator.WriteClassBase("PlaceBase","PlaceBase.cs", dr);


            //string sampleshpfile = @"C:\Users\John\Documents\QrCode\Input\Places2019\Places\tl_2019_01_place.shp";

            //ShapeFile s = new ShapeFile(sampleshpfile);

            //s.Load();

            //string[] s = Directory.GetFiles(@"C:\Users\John\Documents\QrCode\ImportShapeFilesAndDBF\ShapeUtilities", "*.cs");

            //var fs = File.Create("ifstatements.cs");
            //var ts = new StreamWriter(fs);

            //foreach (string s1 in s)
            //{
            //    var s2 = Path.GetFileNameWithoutExtension(s1);

            //    ts.WriteLine("if (this is " + s2 + ")");
            //    ts.WriteLine("{");
            //    ts.WriteLine("  var obj=(" + s2 + ")this;");
            //    ts.WriteLine("  sb.Append(\"" + s2.ToUpper().Replace("SHAPE", "") + "(\");");
            //    ts.WriteLine("}");
            //    ts.WriteLine();



            //}

            //ts.Flush();
            //ts.Close();

        }
    }
}
