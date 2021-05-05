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
using System.Configuration;

namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {
            string basedatadir = ConfigurationManager.AppSettings["basedatadir"];


            // still working on the fast resume id portion, so for now we're going to skip the 8 million or so row tablescan

            //AquiferLoader aq = new AquiferLoader(true,false,basedatadir + "AqiferData");
            //aq.LoadZips();

            //RiversLoader rivl = new RiversLoader(true, false, basedatadir + "RiversAndStreamsData");
            //rivl.LoadZips();

            //PlaceLoader pl = new PlaceLoader(true, false, basedatadir + "PlacesZips");
            //pl.LoadZips();

            //RoadsLoader rl = new RoadsLoader(true, false, basedatadir + "RoadsZips");
            //rl.LoadZips();

            //CountyLoader cl = new CountyLoader(true, false, basedatadir + "CountyZips");
            //cl.LoadZips();

            //StateLoader st = new StateLoader(true, false, basedatadir + "StateZips");
            //st.LoadZips();

            SpeciesSeasonLoader ssl = new SpeciesSeasonLoader(true, false, basedatadir + "SpeciesData\\repack");
            ssl.LoadZips();
        }
    }
}
