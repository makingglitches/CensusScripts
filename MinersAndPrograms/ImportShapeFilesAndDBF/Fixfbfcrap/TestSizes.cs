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

    // Oh what a perfect wonderful day
    // the women had large breasts
    // and i got everything i wanted with no annoyance.

    public class TestSizes
    {
        private string filename;

        /// <summary>
        /// The size of the file specified by filename.
        /// </summary>
        public double RasterSize = 0;

        GDALRead.TimePieces[,] reports;

        public TestSizes(string fname)
        {
            filename = fname;
        }

        public class TimeToLen
        {
            public int TileSize=0;
            public long PngSize=0;
            public long TiffSize=0;
            public TimeSpan TiffTime = TimeSpan.Zero;
            public TimeSpan PngTime = TimeSpan.Zero;
        }

        public void Start()
        {
            Console.WriteLine("Compression Size Test For Specific Raster");

            FileInfo f = new FileInfo(filename);
            FileInfo f2 = new FileInfo(filename.Substring(0, filename.Length - 4)+".ige");
            Console.WriteLine("Present Filename: " + filename);
            Console.WriteLine("Size: " + ( (f.Length+f2.Length) / 1024 / 1024).ToString() + " Mb");

            GDALRead r = new GDALRead(filename);
            r.OpenFile();
            int[] sizes = new int[] {512,1024,2048,4096 };

            List<TimeToLen> measures = new List<TimeToLen>();
            

            for (int s = 0; s < sizes.Length; s++)
            {
                Console.WriteLine("Processing image tiles of size {0}", sizes[s]);
                
                Console.WriteLine("Size in Tiles: {0} x {1}", r.XTiles(sizes[s]), r.YTiles(sizes[s]));
                
                int xtiles = r.XTiles(sizes[s]);
                int ytiles = r.YTiles(sizes[s]);

                reports = new GDALRead.TimePieces[xtiles,ytiles];

                var t = r.GetTileTest(xtiles-1, ytiles-1, 512);

                TimeToLen measure = new TimeToLen();

                measure.TileSize = sizes[s];

                measures.Add(measure);

                for (int x = 0; x < xtiles; x++)
                {
                    for (int y = 0; y < ytiles; y++)
                    {
                        Console.CursorLeft = 0;
                        Console.Write("                                        ");
                        Console.CursorLeft = 0;
                        Console.Write("Processing Tile {0}, {1}", x, y);

                        var test = r.GetTileTest(x, y, sizes[s]);
                        measure.PngSize += t.PngSize;
                        measure.TiffSize += t.TiffSize;
                        measure.PngTime = measure.PngTime + t.PngTime;
                        measure.TiffTime = measure.TiffTime + t.TiffTime;
                        reports[x, y] = test;
                    }
                }

          
            }

            Console.WriteLine("Calulated Sizes.");

            Console.WriteLine("\tSize\tPNG Time\tPng Size\tTiff Time\tTiff Size");

            for (int x=0; x< measures.Count; x++)
            {
                Console.WriteLine("{0}\t{1}\t{2} mb\t{3}\t{4} mb",
                    measures[x].TileSize, 
                    measures[x].PngTime, 
                    measures[x].PngSize/1024/1024, 
                    measures[x].TiffTime, 
                    measures[x].TiffSize/1024/1024);

            }
            
            Console.WriteLine();
            Console.WriteLine("Test Complete.");
            Console.WriteLine();
            Console.WriteLine();

        }

    }
}
