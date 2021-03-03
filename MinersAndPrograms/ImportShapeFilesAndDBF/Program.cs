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

         //   AquiferLoader aq = new AquiferLoader(basedatadir + "AqiferData", true, true);
          //  aq.LoadZips();

            RiversLoader rivl = new RiversLoader(basedatadir + "RiversAndStreamsData", false, true,true);
            rivl.LoadZips();

            PlaceLoader pl = new PlaceLoader(basedatadir + "PlacesZips", true, true);
            pl.LoadZips();

            RoadLoader rl = new RoadLoader(basedatadir + "RoadsZips", true, true);
            rl.LoadZips();

            CountyLoader cl = new CountyLoader(basedatadir + "CountyZips", true, true);
            cl.LoadZips();

            StateLoader st = new StateLoader(basedatadir + "StateZips", true, true);
            st.LoadZips();


        }
    }
}
