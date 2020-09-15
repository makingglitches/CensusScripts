using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
    public class MultiPointMShape:MultiPointShape
    {
        public double MinM { get; set; }
        public double MaxM { get; set; }

        List<double> MArray { get; set; }
        public MultiPointMShape(BinaryReader br):base(br)
        {
            MinM = br.ReadDouble();
            MaxM = br.ReadDouble();
            MArray = ReadDoubles(br, NumberPoints);
        }
    }
}
