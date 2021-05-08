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
using RasterStats.Tests;
using RasterStats.Stats;

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

            // how to select that is the most representative eh ?
            // 3600 values to make this very simple of value seperation.
            // stdev is a nice indicator but can be skewed.

            TestVariances tv = new TestVariances(file);
            tv.Start();

            List<TileVariance> vars = new List<TileVariance>();

            // transform into a flat list.
            tv.variances.ForEach(o => o.ForEach(o2 => vars.Add(o2)));

           
            vars.Sort( (x,y)=>
            {
                if (x.StdDev > y.StdDev) return 1;
                else if (x.StdDev < y.StdDev) return -1;
                else return 0;
            });

            //var bystdev = vars.GroupBy(o => o.StdDev, o => o);


            // processing time currently about 1 per second
            double secs = vars.Count / 60.0 / 60.0;

            double samplesize = Math.Ceiling(vars.Count / secs);


            Console.WriteLine("At earlier test speed, encoding to two formats took a second each.");
            Console.WriteLine("For the present raster that equals {0} hours.", secs);
            Console.WriteLine("To reduce this down to an hour. Record count must reduce to: {0} of {1}", samplesize, vars.Count);

    

            var maxstdev = vars.Max(o => o.StdDev);
            var minstdev = vars.Min(o => o.StdDev);

            // i need to get a sampling of rate from so many repeats between certain saturation values.
            // now percentages will always add up to 100%, so min-max should place samples in a certain range
            // stddev is likely not the best way to group but it is quick and likely it generally will be representative enough
            // all but the last row and last column are the same size in the samples.
            // i have a feeling i'll have to make some arbitrary divosr.
 
            // distribute across one percentage each 
            // most of them are of unique size except stdev of 0.
            var panelsize = (maxstdev - minstdev) / 100;

            double startpanel = minstdev;

            Console.WriteLine("Dividing stdev range into panels, panel size was: {0}", panelsize);

            Dictionary<double, List<TileVariance>> panels = new Dictionary<double, List<TileVariance>>();
            Dictionary<double, long> panelcounts = new Dictionary<double, long>();



            while (startpanel < maxstdev)
            {
                var panel =vars.Where(o => o.StdDev >= startpanel && o.StdDev < startpanel + panelsize).ToList();
                
                long c = 0;

                panel.Sort((a, b) =>
                {
                    if (a.StdDev > b.StdDev) return 1;
                    else if (a.StdDev < b.StdDev) return -1;
                    else return 0;
                });

                c = panel.Select(o => o.Count).Sum();

                panels.Add(startpanel, panel);

                panelcounts.Add(startpanel, c);

                startpanel += panelsize;
                
            }

            long totalc = panelcounts.Select(o => o.Value).Sum();

            Dictionary<double, int> parts = new Dictionary<double, int>();

            foreach (var c in panelcounts)
            {
                parts.Add(c.Key, (int)Math.Ceiling((double)c.Value / (double)totalc * samplesize));
            }

            Dictionary<double, List<TileVariance>> selections = new Dictionary<double, List<TileVariance>>();

            Random r = new Random();



            foreach (var c in parts)
            {
                List<TileVariance> samples = new List<TileVariance>();

                for (int i = 0; i < c.Value; i++)
                {
                    var sampleindex = (int)Math.Floor(r.NextDouble() * c.Value);
                    samples.Add(panels[c.Key][sampleindex]);
                }

                selections.Add(c.Key, samples);
            }


            TestSizes ts = new TestSizes(file,selections);

            // start test sizes test
            ts.Start(512);

            double sampleratio = (double)vars.Count / samplesize;

           

            Console.WriteLine("Panel Start\tPanel End\tCopy Time\tPng Time\tTiff Time\tPng Size\tTiff Size\t");

            foreach ( var c in ts.report)
            {
                c.Value.CopyTime =  new TimeSpan(0,0, (int) (c.Value.CopyTime.TotalSeconds * sampleratio));
                c.Value.PngTime = new TimeSpan(0, 0, (int)(c.Value.PngTime.TotalSeconds * sampleratio));
                c.Value.TiffTime = new TimeSpan(0, 0, (int)(c.Value.TiffTime.TotalSeconds * sampleratio));
                c.Value.PngSize = (long)(c.Value.PngSize * sampleratio);
                c.Value.TiffSize = (long)(c.Value.TiffSize * sampleratio);
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t",c.Key, c.Key + panelsize, c.Value.CopyTime, c.Value.PngTime, c.Value.TiffTime, c.Value.PngSize, c.Value.TiffSize);
            }

            Console.WriteLine("Process finished.");

        }

    }
}
