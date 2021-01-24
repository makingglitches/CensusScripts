using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.IO;
using ShapeUtilities;
using CensusFiles;


namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {
            string sampledbffile = @"C:\Users\John\Documents\QrCode\Input\Places2019\Places\tl_2019_01_place.dbf";

            DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(sampledbffile);

            PlaceRecord pr = new PlaceRecord();
            dr.Read();
            pr.Read(dr);


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
