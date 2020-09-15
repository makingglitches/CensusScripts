using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
   public class ShpPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public ShpPoint(BinaryReader br)
        {
            X = br.ReadDouble();
            Y = br.ReadDouble();
        }
    }
}
