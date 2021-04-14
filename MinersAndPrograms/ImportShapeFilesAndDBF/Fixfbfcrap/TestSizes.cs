using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using OSGeo.GDAL;
using System.IO;
using System.Text.Json;

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

        public List<List<List<GDALRead.TimePieces>>> reports;

        public List<TimeToLen> measures = new List<TimeToLen>();

        public readonly int[] sizes = { 512, 1024, 2048, 4096 };

        public TestSizes(string fname)
        {
            filename = fname;
        }

        public class TimeToLen
        {
            public TimeToLen()
            {
                TileSize = 0;
                PngSize = 0;
                TiffSize = 0;
                TiffTime = TimeSpan.Zero;
                PngTime = TimeSpan.Zero;
                OfMeasures = 0;

            }

            public long OfMeasures { get; set; } 
            public int TileSize { get; set; }
            public long PngSize { get; set; }
            public long TiffSize { get; set; }
            public TimeSpan TiffTime { get; set; }
            public TimeSpan PngTime { get; set; }
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

            //List<TimeToLen> testarr = new List<TimeToLen>();


            //// well.. that is hella effective ! leaps and bounds over .net 3.5 and .net 4.0 !
            //TimeToLen t1 = new TimeToLen() { PngSize = 1, TiffSize = 2, TiffTime = TimeSpan.Zero, PngTime = TimeSpan.Zero, TileSize = 512 };

            //testarr.Add(t1);
            //testarr.Add(t1);
            //testarr.Add(t1);

            //string jsonstring = JsonSerializer.Serialize(testarr);
            //var backwards = JsonSerializer.Deserialize(jsonstring, typeof(List<TimeToLen>));


            //// my awful array isn't supported but this awful multitier list is lol
            //List<List<List<GDALRead.TimePieces>>> tps = new List<List<List<GDALRead.TimePieces>>>();

            //tps.Add(new List<List<GDALRead.TimePieces>>());
            //tps[0].Add(new List<GDALRead.TimePieces>());

            //GDALRead.TimePieces tp =
            //    new GDALRead.TimePieces { CopyTime = TimeSpan.Zero, PngSize = 1, PngTime = TimeSpan.Zero, TiffSize = 1, TiffTime = TimeSpan.Zero };
            //tps[0][0].Add(tp);

            //tps.Add(new List<List<GDALRead.TimePieces>>());
            //tps[1].Add(new List<GDALRead.TimePieces>());
            //tps[1].Add(new List<GDALRead.TimePieces>());

            //tps[1][0].Add(tp);
            //tps[1][1].Add(null);
            //tps[1][1].Add(tp);


            //string s2 = JsonSerializer.Serialize(tps);

            //List<List<List<GDALRead.TimePieces>>> resurrect = new List<List<List<GDALRead.TimePieces>>>();

            //resurrect = (List<List<List<GDALRead.TimePieces>>>) JsonSerializer.Deserialize(s2, typeof(List<List<List<GDALRead.TimePieces>>>));


            if (!File.Exists("measures.json"))
            {
                File.Create("measures.json").Close();
            }
            else
            {
                var mstring = File.ReadAllText("measures.json");

                if (!string.IsNullOrEmpty(mstring))
                {
                    measures = (List<TimeToLen>)JsonSerializer.Deserialize(mstring, typeof(List<TimeToLen>));
                }
                else
                {
                    measures = new List<TimeToLen>();
                }
            }

            if (!File.Exists("times.json"))
            {
                File.Create("times.json");
            }
            else
            {
                var tstring = File.ReadAllText("times.json");

                if (!string.IsNullOrEmpty(tstring))
                {
                    reports = (List<List<List<GDALRead.TimePieces>>>)
                        JsonSerializer.Deserialize(tstring, typeof(GDALRead.TimePieces[][,]));
                }
                else
                {
                    reports = new List<List<List<GDALRead.TimePieces>>>();
                }
            }

            // this is resume logic
            // this process will take somewhere around a minimum of 20 hours to complete just the 512 calculation
            // based off one tile per second.
            // what we'll get is running statistics.
            // I could make this and probably will this time
            // more accurate on average by randomly selecting which rows and columns to process 
            // on each pass, until they're all finished
            // this could be accomplished with a masking array.
           
            // start at the last recorded test.
            int startingsize =  measures.Count==0 ?0: measures.Count - 1;
           
            
            for (int s = startingsize; s < sizes.Length; s++)
            {
                Console.WriteLine("Processing image tiles of size {0}", sizes[s]);
                
                Console.WriteLine("Size in Tiles: {0} x {1}", r.XTiles(sizes[s]), r.YTiles(sizes[s]));
                
                int xtiles = r.XTiles(sizes[s]);
                int ytiles = r.YTiles(sizes[s]);

                if (reports[s] == null)
                {
                    reports[s] = new List<List<GDALRead.TimePieces>>();
                };

                //var t = r.GetTileTest(xtiles-1, ytiles-1, 512);

                TimeToLen measure;

                if (measures.Count < s + 1)
                {
                    measure = new TimeToLen();

                    measure.TileSize = sizes[s];

                    measures.Add(measure);
                }
                else
                {
                    measure = measures[s];
                }

                int startx = -1;
                int starty = -1;

                while (reports[s][startx+1,starty+1]!=null)
                {
                    starty++;

                    if (starty == ytiles-1 )
                    {
                        starty = -1;
                        startx++;
                    }
                }

                startx = startx == -1 ? 0 : startx;
                starty = starty == -1 ? 0 : starty;
               

                for (int x = startx; x < xtiles; x++)
                {
                    for (int y=starty = 0; y < ytiles; y++)
                    {
                        Console.CursorLeft = 0;
                        Console.Write("                                        ");
                        Console.CursorLeft = 0;
                        Console.Write("Processing Tile {0}, {1}", x, y);

                        var test = r.GetTileTest(x, y, sizes[s]);

                        measure.OfMeasures++;
                        measure.PngSize += test.PngSize;
                        measure.TiffSize += test.TiffSize;
                        measure.PngTime = measure.PngTime + test.PngTime;
                        measure.TiffTime = measure.TiffTime + test.TiffTime;
                        
                        reports[s][x, y] = test;

                        File.WriteAllText("measures.json", JsonSerializer.Serialize(measures));
                        File.WriteAllText("times.json", JsonSerializer.Serialize(reports));

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

            Console.WriteLine("Writing full measurement results to file: measures.json and timepieces.json");
 
            Console.WriteLine();
            Console.WriteLine("Test Complete.");
            Console.WriteLine();
            Console.WriteLine();

        }

    }
}
