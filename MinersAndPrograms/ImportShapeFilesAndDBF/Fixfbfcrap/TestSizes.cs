using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using OSGeo.GDAL;
using System.IO;

namespace Fixfbfcrap
{
    public class TestSizes
    {
        private string filename;
        public TestSizes(string fname)
        {
            filename = fname;
        }

        public void Start()
        {
            Console.WriteLine("Compression Size Test For Specific Raster");

            FileInfo f = new FileInfo(filename);

            Console.WriteLine("Present Filename: " + filename);
            Console.WriteLine("Size: " + (f.Length / 1024 / 1024).ToString() + " Mb");

            Gdal.Open(filename, Access.GA_ReadOnly);

        }
            
    }
}
