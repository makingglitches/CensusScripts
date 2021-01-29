using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public class PointMShape:PointShape
    {
        public double M { get; set; }
        public PointMShape(BinaryReader br):base(br)
        {
            M = br.ReadDouble();
        }
    }
}
