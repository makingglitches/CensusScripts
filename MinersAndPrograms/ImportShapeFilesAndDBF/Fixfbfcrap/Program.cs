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

            TestVariances tv = new TestVariances(file,30000);
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

            Console.WriteLine("At earlier test speed, encoding to two formats took a second each.");
            Console.WriteLine("For the present raster that equals {0} hours.", secs);
            Console.WriteLine("To reduce this down to an hour. Record count must reduce to: {0} of {1}", Math.Ceiling(vars.Count / secs), vars.Count);

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

                panels.Add(startpanel, panel);

                panelcounts.Add(startpanel, c);

                startpanel += panelsize;
                
            }

            


            TestSizes ts = new TestSizes(file);

            // start test sizes test
            ts.Start();

        }

    }
}
