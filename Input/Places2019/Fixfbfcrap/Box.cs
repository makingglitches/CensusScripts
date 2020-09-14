using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
    public class ShpBox
    {
        public double XMin { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public double XMax { get; set; }
        public ShpBox (BinaryReader br)
        {
            XMin = br.ReadDouble();
            YMin = br.ReadDouble();
            XMax = br.ReadDouble();
            YMax = br.ReadDouble();
        }
    }
}
