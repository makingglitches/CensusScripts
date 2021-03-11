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

namespace Fixfbfcrap
{
    class Program
    {

        // god i love source control
        
        static void Main(string[] args)
        {

            XmlDocument x = new XmlDocument();
            
          
            x.Load("bCOHAx_CONUS_Range_2001v1.xml");

            var titlenode = x.SelectSingleNode("/metadata/idinfo/citation/citeinfo/title");


            string speciesinfo = titlenode.InnerText;
            string commonname = speciesinfo.Substring(0, speciesinfo.IndexOf("("));
            string latinname = speciesinfo.Substring(speciesinfo.IndexOf("(") + 1,  speciesinfo.IndexOf(")") -  speciesinfo.IndexOf("(") -1  );

            Console.WriteLine(commonname);
            Console.WriteLine(latinname);

        }
       
    }
}
