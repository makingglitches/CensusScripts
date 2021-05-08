using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RasterStats.Stats;

namespace RasterStats.Tests
{
  public  class TestVariances
    {
        string fname;

        public List<List<TileVariance>> variances;
        int lim;

        public TestVariances(string filename, int limit =0)
        {
            fname = filename;
            lim = limit;
        }

        public void Start()
        {
            GDALRead gr = new GDALRead(fname);
            gr.OpenFile();

            var start = DateTime.Now;

            variances = TileVariance.GetTileVariances(gr,512,lim);

            var endd = DateTime.Now;


            // why ? because I want to store the results in something I can open, there is no text to worry about
            // json is bloated and the serializer bombs and if I'm gonna do row by row might as well do this.
            // since i'll be generating this all but once per raster and using the results directly.


            Console.WriteLine("Writing Variance Data To File.");

            var fs = File.Create("bytevariance.txt");

            StreamWriter s = new StreamWriter(fs);

            s.Write("X\tY\tCount\tTileSize\tStdDev\tAvg\tMax Percentage\tMin Percentage\t");

            for (int x=0; x < 256; x++)
            {
                s.Write(x.ToString() + "\t");
            }

            s.WriteLine();


            for (int y=0; y < variances.Count; y++)
            {
                var vy = variances[y];

                for (int x=0; x< vy.Count; x++)
                {
                    var item = vy[x];

                    item.WriteRow(s);
                   
                }
            }

            fs.Flush();
            fs.Close();
            
            Console.WriteLine("Calculation of Variances took: " + (endd - start).ToString());
        }


    }
}
