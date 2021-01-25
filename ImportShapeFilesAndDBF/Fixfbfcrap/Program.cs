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


namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {


            SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder();
            scb.InitialCatalog = "Geography";
            scb.IntegratedSecurity = true;

            SqlConnection scon = new SqlConnection(scb.ConnectionString);
            scon.Open();

            string sampledbffile = @"C:\Users\John\Documents\CensusProject\QrCode\Input\Places2019\Places\tl_2019_01_place.dbf";

            var places = PlaceRecord.ParseDBFFile(sampledbffile,scon);



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
