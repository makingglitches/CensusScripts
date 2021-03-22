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
using System.Xml;

namespace Fixfbfcrap
{
    class Program
    {

        // god i love source control
        
        static void Main(string[] args)
        {


            // this is due to conus albers projection, not data error, working on getting gdal built because of config mismatches
            // with swig libs packaged, or fixing that issue, so hopefully conversions can occur more easily
            // also to get the tree canopy and other elevation data file readers set up.
            // the img format with ige spillfill is not compressed AT ALL. and it very likely could be losslessly compressed !
            //string zipdir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\SpeciesData\repack";
            //string sfs = @"c:\testdata";

            //string[] shpfiles = Directory.GetFiles(sfs, "*.shp");

            //foreach (string f in shpfiles)
            //{
            //    ShapeFile s = new ShapeFile(f);
            //    s.Load();

            //    Console.Write(f);

            //    int countlon = 0;
            //    int countlat = 0;

            //    for (int x = 0; x < s.Records.Count; x++)
            //    {
            //        PolygonShape ps = (PolygonShape)s.Records[x].Record;

            //        for (int y = 0; y < ps.NumPoints; y++)
            //        {

            //            countlon += (ps.Points[y].X > 180 || ps.Points[y].X < -180) ? 1 : 0;
            //            countlat += (ps.Points[y].Y > 90 || ps.Points[y].Y < -90) ? 1 : 0;
                     
            //        }

            //    }

            //    if (countlat > 0 || countlon > 0)
            //    {
            //        Console.WriteLine("File Contained " + countlat.ToString() + " bad latitude points and " + countlon.ToString() + " bad longitude points.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("File within bounds");
            //    }

            //}

            //string f = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\SpeciesData\repack\temp\contents\mBCPMx_CONUS_Range_2001v1.dbf";

            //DbfDataReader.DbfDataReader db = new DbfDataReader.DbfDataReader(f);

            //ClassGenerator.WriteClassBase("SpeciesSeasonBase", "SpeciesSeasonBase.cs", db);

            // takes awhile to run...
            //string indir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\SpeciesData";
            //LoaderOptions op = new LoaderOptions();

            //SpeciesRepackager.Repackage(indir, indir + "\\repack", op.ConnectionString);

        }
       
    }
}
