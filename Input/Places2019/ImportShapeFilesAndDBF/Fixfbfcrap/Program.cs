using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.IO;


namespace Fixfbfcrap
{
    class Program
    {
        static void Main(string[] args)
        {
            string sampleshpfile = @"C:\Users\John\Documents\QrCode\Input\Places2019\Places\tl_2019_01_place.shp";

            FileStream fs = new FileStream(sampleshpfile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            // they're anticipating me reading in a certain number of bytes.

            SHPHeader sh = new SHPHeader(br);

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                RecordHeader rh = new RecordHeader(br);
            }

            br.Close();


           // previou stub generation code.
           //var names =  Enum.GetNames(typeof (SHPHeader.eShapeType));

           // foreach (string s in names)
           // {
           //     if (s!="NullShape")
           //     {
           //        string content =  File.ReadAllText(@"..\..\NullShape.cs").Replace("Null", s);
           //        File.WriteAllText(s + "Shape.cs",content);
           //     }
           // }
                
           
        }
    }
}
