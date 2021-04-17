using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterStats.Stats
{
    public class TileVariance
    { 

        #region Computation

    public static List<List<TileVariance>> GetTileVariances(GDALRead activeFile, int tilesize, int testinglimit = 0)
        {
            var remx = activeFile.remainderX(tilesize);
            var remy = activeFile.remaindery(tilesize);

            var tilesx = activeFile.XTiles(tilesize);
            var tilesy = activeFile.YTiles(tilesize);

            List<List<TileVariance>> variances = new List<List<TileVariance>>();

            int count = 0;

            for (int y = 0; y < tilesy; y++)
            {
                int height = tilesy - 1 == y && remy > 0 ? remy : tilesize;

                variances.Add(new List<TileVariance>());

                for (int x = 0; x < tilesx; x++)
                {

                    if (count > testinglimit && testinglimit > 0) break;
                    count++;

                    Console.CursorLeft = 0;
                    Console.Write("                                      ");
                    Console.CursorLeft = 0;
                    Console.Write("Processing Tile: {0} x {1} #{2}", x, y,count);

                    int width = tilesx - 1 == x && remx > 0 ? remx : tilesize;

                    var v = GetVariance(activeFile,1, x * tilesize, y * tilesize, width, height);

                    v.x = x;
                    v.y = y;
                    v.TileSize = tilesize;

                    variances[y].Add(v);
                }

                if (count > testinglimit && testinglimit > 0) break;
            }



            return variances;
        }

        /// <summary>
        /// Gets the variance (number of each byte value) in a tile sampled from the raster
        /// </summary>
        /// <param name="band"></param>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static TileVariance GetVariance(GDALRead activeFile, int band, int startx, int starty, int width, int height)
        {
            TileVariance test = new TileVariance();

            ulong sum = 0;
            long count = 0;

            byte[] bytes = new byte[width * height];

            var bandp = activeFile.RasterImg.GetRasterBand(band);

            bandp.ReadRaster(startx, starty, width, height, bytes, width, height, 0, 0);


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    byte currval = bytes[y * width + x];

                    sum += currval;
                    count++;

                    /// errh oh need to update this to search now that I decided to make this 
                    /// json friendly.
                    /// 
                    int index = test.byteIndex.ContainsKey(currval) ? test.byteIndex[currval] : -1;

                    if (index == -1)
                    {
                        test.Spread.Add(new SpreadValue() { B = currval, C = 1 });
                        test.byteIndex.Add(currval, test.Spread.Count - 1);
                    }
                    else test.Spread[index].C++;
                }
            }

            test.Count = count;
            test.CeilAvg = (int)Math.Ceiling((double)sum / count);

            double va = 0;

            var ind = test.byteIndex.ToList();

            double maxp = double.MinValue;
            double minp = double.MaxValue;

            for (int x = 0; x < ind.Count; x++)
            {
                int index = ind[x].Value;

                if (index > -1)
                {
                    SpreadValue val = test.Spread[index];
                    // this is the variance sum
                    va += Math.Pow((double)val.B - (double)test.CeilAvg, 2.0) * (double)val.C;
                    // these will end up in the same order. however if there is no value it will leave wholes between and throw index out of rang 
                    // exceptions if the index field isn't being checked.

                    PercSpread p = new PercSpread()
                    {
                        B = val.B,
                        P = (double)val.C / (double)test.Count
                    };

                    test.Percentages.Add(p);

                    if (p.P > maxp)
                    {
                        maxp = p.P;
                    }

                    if (p.P < minp)
                    {
                        minp = p.P;
                    }

                }

            }

            test.MinPercentage = minp;
            test.MaxPercentage = maxp;
            
            // for compression purposes variance matters in that the more values that are the same the higher the probability the file will
            // compress smaller
            // though if a lot of patterns repeat this may not always be the case.
            test.StdDev = Math.Sqrt(va / test.Count);

            test.NormalizedValuesRanges = new List<NormalValue>();

            // extreme outliers
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = double.MinValue, EndDev = -3.0, Count = 0 });
          
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = -3.0, EndDev = -2.0, Count = 0 });
            
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = -2.0, EndDev = -1.0, Count = 0 });
            
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = -1.0, EndDev = 0.0, Count = 0 });

            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = 0.0, EndDev = 0.0, Count = 0 });

            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = 0.0, EndDev = 1.0, Count = 0 });

            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = 1.0, EndDev = 2.0, Count = 0 });
            
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = 2.0, EndDev = 3.0, Count = 0 });
            
            test.NormalizedValuesRanges.Add(new NormalValue() { StartDev = 3.0, EndDev =double.MaxValue, Count = 0 });





            for (int x=0; x< test.Spread.Count; x++)
            {

                SpreadValue val = test.Spread[x];

                if (test.StdDev != 0)
                {
                    val.StdDevs = ((double)val.B - (double)test.CeilAvg) / test.StdDev;
                }
                // interpret this as being equal to the mean. likely all the values are the same.
                else val.StdDevs = 0;


                // place them all in the normal distribution
                for (int i = 0; i < test.NormalizedValuesRanges.Count; i++)
                {
                    NormalValue nv = test.NormalizedValuesRanges[i];

                    if (val.StdDevs == 0.0 && nv.StartDev == 0.0 && nv.EndDev == 0.0)
                    {
                        nv.Count += val.C;
                        break;
                    }
                    else if ( val.StdDevs!=0 && val.StdDevs > nv.StartDev && val.StdDevs <= nv.EndDev)
                    {
                        nv.Count += val.C;
                        break;
                    }
                }
               
            }

            return test;
        }


        #endregion Computation

        public TileVariance()
        {
            Spread = new List<SpreadValue>();
            Percentages = new List<PercSpread>();
        }

        #region StatsFields
        /// <summary>
        /// Lowest percentage of byte values that are the same.
        /// </summary>
        public double MinPercentage { get; set; }

        /// <summary>
        /// Highest percentage of byte values that are the same
        /// </summary>
        public double MaxPercentage { get; set; }

        public double StdDev { get; set; }
        public long Count { get; set; }
        public int CeilAvg { get; set; }
        public int TileSize { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        /// <summary>
        /// this just keeps the byte indexes in a nice order for referencing during generation. the others are in first-encountered order.
        /// </summary>
        public Dictionary<byte, int> byteIndex = new Dictionary<byte, int>();

        public List<SpreadValue> Spread { get; set; }
        public List<PercSpread> Percentages { get; set; }

        public List<NormalValue> NormalizedValuesRanges { get; set; }

        #endregion StatsFields
    }

}
