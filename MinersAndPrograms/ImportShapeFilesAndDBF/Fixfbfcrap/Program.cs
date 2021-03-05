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


namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {
            LoaderOptions l = new LoaderOptions()
            {
                TableName = "Rivers",
                EmptyTable = false,
                Resume=true,
                DbaseResumeId = "OBJECTID",
                SqlResumeId = "ObjectId",
                RecordLimit=1000,
                FileDirectory = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\RiversAndStreamsData"
            };

            GenericLoader g = new GenericLoader(l);
            g.GetNewRecord = ()=> (IRecordLoader)new RiversRecord() ;
            g.ProcessRecord += G_ProcessRecord;
            g.SkipRecord += G_SkipRecord;
            g.Status += G_Status;
            g.OnLength += G_OnLength;

            g.LoadZips();

            #region OldCommentedCode

            //SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder();
            //scb.InitialCatalog = "Geography";
            //scb.IntegratedSecurity = true;

            //SqlConnection scon = new SqlConnection(scb.ConnectionString);
            //scon.Open();

            //DataTable dt = RiversRecord.GetTable(scon);

            //SqlBulkCopy sb = new SqlBulkCopy(scon);

            //sb.DestinationTableName = "Rivers";
            //sb.WriteToServer(dt);

            //scon.Close();

            //// ok lets look into record 272
            //var shp = new ShapeFile(@"C:\Users\John\Documents\CensusProject\Issues\tl_2019_01_place\tl_2019_01_place.shp");
            //shp.Load();

            //var r = shp.Records[271];

            //string wkt = r.Record.GetWKT();

            //PolygonShape p = (PolygonShape)r.Record;

            //StreamWriter sw = new StreamWriter("Citronell Point Data.txt");

            //if (p.NumParts==1)
            //{
            //    sw.WriteLine("File only has one part.");
            //}

            //sw.WriteLine("Shape contains " + p.NumPoints.ToString() + " points.");
            //sw.WriteLine("with " + p.NumParts.ToString() + " parts.");

            //int partindex = 0;

            //for (int x=0; x < p.NumPoints; x++)
            //{
            //    if (partindex < p.NumParts &&  x==p.Parts[partindex])
            //   {
            //        sw.WriteLine();
            //        sw.WriteLine("Part starting at point #" + x);
            //        partindex++;
            //   }
            //    sw.WriteLine(p.Points[x].X.ToString() + ", " + p.Points[x].Y.ToString());
            //}

            //sw.WriteLine();
            //sw.WriteLine(wkt);
            //sw.Flush();
            //sw.Close();



            //var dread = new DbfDataReader.DbfDataReader(@"C:\Users\John\Documents\CensusProject\AqiferData\aquifrp025.dbf");



            //ClassGenerator.WriteClassBase("AquiferBase", "AquiferBase.cs", dread);

            //var shp = new ShapeFile(@"C:\Users\John\Documents\CensusProject\AqiferData\aquifrp025.shp");
            //shp.Load();

            //var dread = new DbfDataReader.DbfDataReader(@"C:\Users\John\Documents\CensusProject\RiversAndStreamsData\USA_Rivers_and_Streams-shp\9ae73184-d43c-4ab8-940a-c8687f61952f2020328-1-r9gw71.0odx9.dbf");



            //ClassGenerator.WriteClassBase("RiversBase", "RiversBase.cs", dread);

            //var shp = new ShapeFile(@"C:\Users\John\Documents\CensusProject\RiversAndStreamsData\USA_Rivers_and_Streams-shp\9ae73184-d43c-4ab8-940a-c8687f61952f2020328-1-r9gw71.0odx9.shp");
            //shp.Load();

            //var dread = new DbfDataReader.DbfDataReader(@"C:\Users\John\Documents\CensusProject\CensusShapeFileData\StateZips\tl_2019_us_state\tl_2019_us_state.dbf");


            //ClassGenerator.WriteClassBase("StateBase", "StateBase.cs", dread);

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
            #endregion OldCommentedCode
        }

        private static long _length;
        private static int x;
        private static int y;
        private static long consolewid;
        private static string BlankLine;

        private static void G_OnLength(GenericLoader g, string dbffilename, string shpfilename, long length)
        {
            _length = length;
            
            Console.WriteLine("Processing Dbase: " + Path.GetFileName( dbffilename));
            Console.WriteLine("Processing shp: " + Path.GetFileName(shpfilename));
            Console.WriteLine("Discovered " + length.ToString() + " Records.");
            
            if ( g.Options.Resume) 
            { 
                Console.WriteLine("===> Resume On <==="); 
            }

            x = Console.CursorLeft;
            y = Console.CursorTop;
            consolewid = Console.LargestWindowWidth;

            BlankLine = string.Empty;

            for (int rep=0; rep < 80; rep++)
            {
                BlankLine += " ";
            }
        }


        // they are actually fearful of being caught but they screw up other people by acting like people will misinterpret things
        // and then pretending until someone grabs them by their head and makes the best use of their mouth
        // in general the idea is to pressure everyone not only to acting a certain way in their WEIRD trash system
        // but also to restructure the minds of people not like them if they tell them to screw their emotions up.

        private static void G_Status(int index, int wrote, int writing, double rate)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(BlankLine);
            Console.WriteLine(BlankLine);

            Console.SetCursorPosition(x, y);
            Console.WriteLine("Processing record " + (index+1).ToString() + " of " + _length.ToString());
            Console.WriteLine((writing>0?"===>WRITING:"+writing.ToString()+" <====":"")+ "Wrote: "+wrote.ToString()+" to Database @ "+rate.ToString()+" r/s");
        }

        private static void G_SkipRecord(GenericLoader g, int index, IRecordLoader r, BaseRecord shape, int wrote)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(BlankLine);
            Console.WriteLine(BlankLine);
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Skipping " + index.ToString() + " of " + _length.ToString());
        }

        private static void G_ProcessRecord(GenericLoader g, int index, IRecordLoader r, BaseRecord shape, int wrote)
        {
            // nothing to do for rivers record
            // but here one might process fips codes etc
            // however may migrate all that to the sql scripts collection for after the main load event.
            // since everything needed is pretty much there.

            // annndd a use afterall.
            var r1 = (RiversRecord)r;
            r1.ShapeInfo = (PolyLineShape) shape;
        }
    }
}
