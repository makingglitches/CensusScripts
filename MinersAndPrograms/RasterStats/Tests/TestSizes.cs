using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using RasterStats.Stats;

namespace RasterStats.Tests
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

        public Dictionary<double, TimePieces> report;

        public readonly int[] sizes = { 512, 1024, 2048, 4096 };

        public Dictionary<double, List<TileVariance>> Selections;

        public TestSizes(string fname, Dictionary<double, List<TileVariance>> selections = null)
        {
            filename = fname;
            Selections = selections;
        }



        public void Start(int tilesize)
        {
            Console.WriteLine("Compression Size Test For Specific Raster");

            // this is set up for erdas imagine with a spillfile.
            FileInfo f = new FileInfo(filename);
            FileInfo f2 = new FileInfo(filename.Substring(0, filename.Length - 4) + ".ige");

            Console.WriteLine("Present Filename: " + filename);
            Console.WriteLine("Size: " + ((f.Length + f2.Length) / 1024 / 1024).ToString() + " Mb");

            GDALRead r = new GDALRead(filename);
            r.OpenFile();

            report = new Dictionary<double, TimePieces>();

            var total = Selections.Select(o => o.Value.Count).Sum();

            Console.WriteLine("Running tests for {0} selections", total);
            Console.WriteLine("Encoding for tile size: {0}", tilesize);

            int counted = 0;

            foreach (var sel in Selections)
            {
                Console.WriteLine("On Panel Start: {0}", sel.Key);

                TimePieces summary = new TimePieces();

                foreach (var l in sel.Value)
                {
                    counted++;

                    Console.CursorLeft = 0;
                    Console.Write("                                                      ");
                    Console.CursorLeft = 0;
                    Console.Write("On sample #{0} of {1}", counted, total);

                    var test = r.GetTileTest(l.x, l.y, tilesize);

                    summary.Pieces.Add(test);
                    summary.OfMeasures++;
                    summary.PngSize += test.PngSize;
                    summary.TiffSize += test.TiffSize;
                    summary.PngTime = summary.PngTime + test.PngTime;
                    summary.TiffTime = summary.TiffTime + test.TiffTime;
                    summary.CopyTime = summary.CopyTime + test.CopyTime;
                }

                report.Add(sel.Key, summary);
                Console.WriteLine();
            }


            Console.WriteLine("Calulated Sizes.");
        }

    }
}
