using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    /// <summary>
    /// Identical to PolyLine shape, interpretation different
    /// </summary>
    public class PolygonShape:PolyLineShape
    {
        public PolygonShape(BinaryReader br):base(br)
        {

        }
    }
}
