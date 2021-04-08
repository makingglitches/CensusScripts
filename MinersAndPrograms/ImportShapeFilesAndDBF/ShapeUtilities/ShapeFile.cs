using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
   public class ShapeFile
    {
        public string FileName { get; set; }

        public SHPHeader FileHeader { get; set; }

        public List<RecordHeader> Records { get; set; }

        public ShapeFile(string fileName)
        {
            FileName = fileName;

            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException("The specified shapefile does not exist", fileName);
            }


        }

        public void Load()
        {
            FileStream fs = new FileStream(FileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            FileHeader = new SHPHeader(br);

            Records = new List<RecordHeader>();

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                RecordHeader rh = new RecordHeader(br);
                Records.Add(rh);
            }

            br.Close();
        }



    }
}
