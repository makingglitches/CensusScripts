using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Data.Linq;
using CensusFiles.Loaders;

namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {
            string basedatadir = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\";

            //   AquiferLoader aq = new AquiferLoader(basedatadir + "AqiferData", true, true);
            //  aq.LoadKeys(true);

            RiversLoader rivl = new RiversLoader(true, false, basedatadir + "RiversAndStreamsData");
            rivl.LoadKeys(true);

            PlaceLoader pl = new PlaceLoader(true, false,basedatadir + "PlacesZips");
            pl.LoadKeys(true);

            RoadsLoader rl = new RoadsLoader(true,false,basedatadir + "RoadsZips");
            rl.LoadKeys(true);

            CountyLoader cl = new CountyLoader(true,false,basedatadir + "CountyZips");
            cl.LoadKeys(true);

            StateLoader st = new StateLoader(true,false,basedatadir + "StateZips");
            st.LoadKeys(true);
        }
    }
}
