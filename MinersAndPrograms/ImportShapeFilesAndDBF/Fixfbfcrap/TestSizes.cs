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

        /// <summary>
        /// The size of the file specified by filename.
        /// </summary>
        public double RasterSize = 0;
        /// <summary>
        /// Publishes a set of keys combined with doubles that identify max resolution and total recode size for png
        /// </summary>
        public Dictionary<int, double> TiffSize = new Dictionary<int, double>();
        public Dictionary<int, TimeSpan> EncodeTime = new Dictionary<int, TimeSpan>();
        /// <summary>
        /// Publishes a set of keys combined with doubles that idenify max resoltion and total record size for tiff
        /// </summary>
        public Dictionary<int, double> PNGSize = new Dictionary<int, double>();
        
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

            GDALRead r = new GDALRead(filename);
            r.OpenFile();
            int[] sizes = new int[] {512,1024,2048,4096 };


            for (int s = 0; s < sizes.Length; s++)
            {
                Console.WriteLine("Processing image tiles of size {0}", sizes[s]);
                
                long currsizepng = 0;
                long currtiffsize = 0;

                TimeSpan copyTime;
                TimeSpan pngTime;
                TimeSpan tiffTime;

                for (int x = 0; x < r.XTiles(sizes[s]); x++)
                {
                    for (int y = 0; y < r.YTiles(sizes[s]); y++)
                    {
                        int tilex = x * sizes[s] > r.RasterImg.RasterXSize ? r.remainderX(sizes[s]) : sizes[s];
                        int tiley = y * sizes[s] > r.RasterImg.RasterXSize ? r.remaindery(sizes[s]) : sizes[s];

                        DateTime start = DateTime.Now;
                        Bitmap b =  r.CopyToBitmap(1, tilex, tilex, x * sizes[s], y * sizes[s], tilex, tiley);
                        DateTime end = DateTime.Now;

                        copyTime = end - start;


                        start = DateTime.Now;

                        MemoryStream ms = new MemoryStream();
                        b.Save(ms, ImageFormat.Png);

                        end = DateTime.Now;
                        pngTime = end - start;

                        currsizepng += ms.Length;
                        
                        start = DateTime.Now;
                        ms = new MemoryStream();

                    //    var codec = ImageCodecInfo.GetImageEncoders().Where(o => o.FormatID.Equals(ImageFormat.Tiff)).First();
                        b.Save(ms, ImageFormat.Tiff);

                        end = DateTime.Now;

                        currtiffsize += ms.Length;

                        tiffTime = start - end;

                    }
                }

                PNGSize.Add(sizes[s], currsizepng);
                TiffSize.Add(sizes[s], currsizepng);

            }

            Console.WriteLine("Calulated Sizes.");

            Console.WriteLine("\tSize\t\tPNG\t\tTiff");

            for (int x = 0; x < sizes.Length; x++)
            {
                Console.WriteLine("\t{0}\t\t{1} Mb\t\t{2} Mb", x, PNGSize[sizes[x]], TiffSize[sizes[x]]);
            }
            Console.WriteLine();
            Console.WriteLine("Test Complete.");
            Console.WriteLine();
            Console.WriteLine();

        }

    }
}
