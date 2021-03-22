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
            string file = @"C:\Users\John\Documents\CensusProject\CensusShapeFileData\TreeCanopy\nlcd_2016_treecanopy_2019_08_31";

            //  SetDllDirectory(@"C:\OSGeo4W64\apps\gdal\csharp\csharp");

            // see its complaining about not finding the dll.
            // i've updated the PATH variable.
            // i've set the path here to search on using the kernel32 wrapper above

            // i changed the directory to location of the dll.
            Environment.CurrentDirectory = @"C:\OSGeo4W64\apps\gdal\csharp\csharp";
            
            //and isnt it interesting it can't find a dll in teh same goddamn directory
            // this is swig generated and i know i got it to work before
            // before they stole this fucking computer.
            // and harddrives as they have been doing over and over since i was younger.
            // now they're just moving forward all the shit they can't push on me so they can fake
            // that there were consequences that dont make sense when they will HAVE to let me go
            // just to traumatize me into forgetting using the weakness john quay sohn introduced 
            // into my head by letting me be raped in fort collins, co when i was a kid.

            // anyway. fuck all of you people.
            Gdal.Open(file, Access.GA_ReadOnly);
      
        }
       
    }
}
