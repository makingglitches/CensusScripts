using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public class PolyLineMShape:PolyLineShape
    {
        public double MinM { get; set; }
        public double MaxM { get; set; }

        public List<double> MArray { get; set;}

        public PolyLineMShape(BinaryReader br):base(br)
        {
            MinM = br.ReadDouble();
            MaxM = br.ReadDouble();
            MArray = ReadDoubles(br, NumPoints);
        }
    }
}
