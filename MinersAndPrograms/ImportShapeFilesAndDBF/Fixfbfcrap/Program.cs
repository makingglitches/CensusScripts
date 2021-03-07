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
        //  i need more fucking ram !
        // course this worked fine yesterday with no resources semaphore wait at approx 80% memory usage
        // at 1000 records per.. so fuck off assholes.
        //... still. ass fucking holes stop dragging me back trying to make me act like the rest of these labotomized assholes are correct !
        // instead of manufacturing crimes to entrap me that make no sense just to delay me, why not do what you people should have done
        // if you were really security minded and just send me some chick to screw and make it so i work in a cabin somewhere writing code in th evenings
        // etc.. jesus, i'd never have complained or noticed anything you people were doing if you werent fucking advertising so hard
        // and additionally no i wont sit here and say extra years of my life werent dragged out of me
        // while the rest of these idiots slowly became basketweaving madmen 
        // seriously though goddamn if you people were actually hiding something you wouldnt just wave it around where anyone can see and say
        // 'whoa... thats really odd and disconcerting and they are acting like TOTAL FUCKING FREAKS'
        // and invent the lie that this is the way it always was and spout that shit to someone else mother fuckers.
        // go hang with my chomo fucking dad if you people want to feel 'normal', he was one of the 1970s loser club.
  
        static void Main(string[] args)
        {
            //string dir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\RiversAndStreamsData";

            //RiversLoader rl = new RiversLoader(true, false, dir);

            //rl.Options.RecordLimit = 100;

            //rl.LoadZips();


            // apparently tori wanted to save time for whatever piece of crap keep stealing my work to 
            // get some rape in. poor poor lad.
            // motherfuckers = them
            // seriously all this waste and time that could have been better spent working on something constructive and they say
            // 'oh we know his daddy, and he wasnt raised to know shit about our true awful fucked up self destructive and purely destructive natures
            // 'i know we can take advantage of him and try to set him up to get stuck so he will be susceptible to our "evil empire" bullshit
            // 'and thereby we can scare or traumatize him into making stuff we steal constantly and then turn shit around so he wont want anything
            // 'later on while we're in the process of fucking ourselves over'
            // which apparnetly they are doing.
            // and the world too.

            //string roaddir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\RoadsZips";

            //RoadsLoader roadl = new RoadsLoader(true, false, roaddir);

            //roadl.Options.RecordLimit = 1000;

            //roadl.LoadZips();


            //string countydir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\CountyZips";
            //CountyLoader col = new CountyLoader(false, true, countydir);
            //col.Options.RecordLimit = 1000;
            //col.LoadZips();

            //string aqudir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\AqiferData";
            //AquiferLoader aql = new AquiferLoader(false, true, aqudir);
            //aql.LoadZips();

            //string placedir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\PlacesZips";
            //PlaceLoader pl = new PlaceLoader(false, true, placedir);
            //pl.Options.RecordLimit = 1000;
            //pl.LoadZips();


            string statedir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\StateZips";
            StateLoader sl = new StateLoader(false, true, statedir);
            sl.LoadZips();

            // nope.. instead i'm sitting in poverty doing my best not to go nutty.
            // which is hard when everyone in a 1 state radius is fucking insane.



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

       
      
        // as much as i get sick of rewriting the same fucking code I get sick of having to constantly attempt to pretend
        // that there is some rhyme or reason to this
        // these people tell half truths mostly but a few whole ones
        // and they live their lives in an artificial circleS
      
       
    }
}
