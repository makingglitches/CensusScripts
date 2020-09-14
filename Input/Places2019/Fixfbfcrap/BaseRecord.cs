using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfDataReader;

namespace Fixfbfcrap
{
   public class BaseRecord
    {

        public int RecordNumber { get; set; }    
        public int ContentLength { get; set; }

        public SHPHeader.eShapeType ShapeType { get; set; }

        public BaseRecord(BinaryReader reader)
        {
            RecordNumber = reader.ReadBigEndianInt32();

            ContentLength = reader.ReadBigEndianInt32();

            ShapeType = (SHPHeader.eShapeType)reader.ReadInt32();

        }

        public static BaseRecord ReadRecord(BinaryReader reader)
        {
            return new NullShape(reader);
        }

        public List<ShpPoint> ReadPoints(BinaryReader br,  int numberpoints)
        {
            List<ShpPoint> retval = new List<ShpPoint>();

            for (int x = 0; x < numberpoints; x++)
            {
                retval.Add(new ShpPoint(br));
            }

            return retval;
        }

        public List<int> ReadParts(BinaryReader br, int numparts)
        {
            List<int> retval = new List<int>();

            for (int x = 0; x < numparts; x++)
            {
                retval.Add(br.ReadInt32());
            }

            return retval;
        }
    }
}
