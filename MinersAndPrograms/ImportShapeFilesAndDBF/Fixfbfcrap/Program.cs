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
using OSGeo.GDAL;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace Fixfbfcrap
{
    class Program
    {

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool SetDllDirectory(string lpPathName);



        static void Main(string[] args)
        {
            // i really hate zimmerman and these people for pushing their non straight agenda to make it harder for
            // men to approach women in this time period unless they're being set up to get in trouble by rotten
            // whores. which likely comprise most of the woemn on the set of the expanse
            // and yeah now remember the end of the season because zimmerman commented on that one.
            // a very long time ago it seems.

            string file = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\TreeCanopy\nlcd_2016_treecanopy_2019_08_31\nlcd_2016_treecanopy_2019_08_31.img";

            Gdal.AllRegister();

            //and isnt it interesting it can't find a dll in teh same goddamn directory
            Dataset d = Gdal.Open(file, Access.GA_ReadOnly);


            var b = d.GetRasterBand(1);
            var ct = b.GetColorTable();
            double[] geo = new double[6];

            d.GetGeoTransform(geo);


            var bounds = new { Left = geo[0], Top = geo[3], xsize = geo[1], ysize = geo[5] };


            byte[] bytes = new byte[512 * 512];


            b.ReadRaster(d.RasterXSize/2, d.RasterYSize/2, 512, 512, bytes, 512, 512, 0, 0);

            System.Drawing.Bitmap dest = new System.Drawing.Bitmap(512, 512);

            Console.WriteLine(ct.GetCount());

            for (int x = 0; x < 512; x++)
            {
                for (int y=0; y < 512; y++)
                {
                    var index = bytes[y * 512 + x];
                    var col = ct.GetColorEntry(index);
                    var c = Color.FromArgb(col.c4, col.c1, col.c2, col.c3);
                    dest.SetPixel(x, y, c);
                }
            }

            
            ImageCodecInfo pnginfo = ImageCodecInfo.GetImageEncoders().Where(o => o.FormatID.Equals(ImageFormat.Png.Guid)).First();

            var compop = System.Drawing.Imaging.Encoder.Compression;
            var qualop = System.Drawing.Imaging.Encoder.Quality;

            
            FileStream fs = new FileStream("test.png", FileMode.Create, FileAccess.Write, FileShare.None);

            dest.Save(fs, ImageFormat.Png);

            fs.Flush();
            fs.Close();

            // seriously days wasted trying to get the libpng library to work in goddamn c++ and it f's up !
            // a few minutes of this and boom it works !

        }

    }
}
