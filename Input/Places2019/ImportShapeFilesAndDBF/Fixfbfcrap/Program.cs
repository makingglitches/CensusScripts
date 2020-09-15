using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.IO;
using ShapeUtilities;


namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {
            string sampleshpfile = @"C:\Users\John\Documents\QrCode\Input\Places2019\Places\tl_2019_01_place.shp";

            ShapeFile s = new ShapeFile(sampleshpfile);

            s.Load();
           
        }
    }
}
