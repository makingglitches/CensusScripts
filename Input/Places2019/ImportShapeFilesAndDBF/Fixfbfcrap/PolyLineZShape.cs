using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
    public class PolyLineZShape:PolyLineShape
    {
        public double ZMin { get; set; }
        public double ZMax { get; set; }
        public List<double> ZArray { get; set; }

        public double MMin { get; set; }
        public double MMax { get; set; }
        public List<double> MArray { get; set; }

        public PolyLineZShape(BinaryReader br):base(br)
        {
            ZMin = br.ReadDouble();
            ZMax = br.ReadDouble();
            ZArray = ReadDoubles(br, NumPoints);
            MMin = br.ReadDouble();
            MMax = br.ReadDouble();
            MArray = ReadDoubles(br, NumPoints);
        }
    }
}
