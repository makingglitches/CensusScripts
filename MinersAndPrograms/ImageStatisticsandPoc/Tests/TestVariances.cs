﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using ImageStatisticsandPoc.Stats;

namespace ImageStatisticsandPoc.Tests
{
  public  class TestVariances
    {
        string fname;

        public List<List<TileVariance>> variances;

        public TestVariances(string filename)
        {
            fname = filename;
        }

        public void Start()
        {
            GDALRead gr = new GDALRead(fname);
            gr.OpenFile();

            var start = DateTime.Now;

            variances = gr.GetTileVariances(512);

            var endd = DateTime.Now;


            // why ? because I want to store the results in something I can open, there is no text to worry about
            // json is bloated and the serializer bombs and if I'm gonna do row by row might as well do this.
            // since i'll be generating this all but once per raster and using the results directly.

            var fs = File.Create("bytevariance.txt");

            StreamWriter s = new StreamWriter(fs);

            s.Write("X\tY\tCount\tTileSize\tStdDev\tAvg\t");

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

                    s.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t", x, y, item.Count, item.TileSize, item.StdDev, item.CeilAvg);

                    for (int b=0; b < 256; b++)
                    {
                        int index = item.byteIndex.ContainsKey((byte)b) ? item.byteIndex[(byte)b] : -1;
                        if (index > -1)
                        {
                            s.Write("{0}-({1})\t", item.Spread[index].C, item.Percentages[index].P);
                        }
                        else
                        {
                            s.Write("-\t");
                        }
                    }

                    s.WriteLine();

                }
            }

            fs.Flush();
            fs.Close();
            
            Console.WriteLine("Calculation of Variances took: " + (endd - start).ToString());
        }


    }
}
