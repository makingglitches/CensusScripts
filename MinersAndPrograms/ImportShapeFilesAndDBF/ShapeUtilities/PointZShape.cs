using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public class PointZShape:PointShape
    {

        public double Z { get; set; }
        public double M { get; set; }

        public PointZShape(BinaryReader br):base(br)
        {
            Z = br.ReadDouble();
            M = br.ReadDouble();
        }
    }
}
