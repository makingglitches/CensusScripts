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

            string file = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\TreeCanopy\nlcd_2016_treecanopy_2019_08_31";

            //  SetDllDirectory(@"C:\OSGeo4W64\apps\gdal\csharp\csharp");

            // see its complaining about not finding the dll.
            // i've updated the PATH variable.
            // i've set the path here to search on using the kernel32 wrapper above

            // i changed the directory to location of the dll.
            Environment.CurrentDirectory = @"C:\OSGeo4W64\apps\gdal\csharp\csharp";
            
            //and isnt it interesting it can't find a dll in teh same goddamn directory
            Gdal.Open(file, Access.GA_ReadOnly);
      
        }
       
    }
}
