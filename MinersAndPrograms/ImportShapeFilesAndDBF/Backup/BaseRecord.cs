using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfDataReader;

namespace ShapeUtilities
{
   public class BaseRecord
    {

        public BaseRecord(BinaryReader reader)
        {
          
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

        public List<double> ReadDoubles(BinaryReader br, int num)
        {
            List<double> retval = new List<double>();

            for (int x=0; x< num;x++)
            {
                retval.Add(br.ReadDouble());
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
