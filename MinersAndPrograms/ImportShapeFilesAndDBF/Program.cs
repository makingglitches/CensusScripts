using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Data.Linq;


namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {

            string basedatadir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\";


            PlaceLoader pl = new PlaceLoader(basedatadir + "PlacesZips",true);
            pl.LoadZips();

            RoadLoader rl = new RoadLoader(basedatadir + "RoadsZips",true);
            rl.LoadZips();
         

        }
    }
}
