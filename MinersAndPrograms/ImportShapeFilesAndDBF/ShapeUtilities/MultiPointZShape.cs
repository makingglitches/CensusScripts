using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public class MultiPointZShape : MultiPointShape
    {

        public double MinZ {get;set;}
        public double MaxZ { get; set; }

        public List<double> ZArray { get; set; }

        public double MinM { get; set; }
        public double MaxM { get; set; }

        public List<double> MArray { get; set; }

        public MultiPointZShape(BinaryReader br):base(br)
        {
            MinZ = br.ReadDouble();
            MaxZ = br.ReadDouble();

            ZArray = ReadDoubles(br, NumberPoints);
            MinM = br.ReadDouble();
            MaxM = br.ReadDouble();

            MArray = ReadDoubles(br, NumberPoints);

        }
    }
}
