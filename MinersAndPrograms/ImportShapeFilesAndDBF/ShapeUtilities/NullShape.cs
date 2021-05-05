using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public class NullShape:BaseShapeRecord
    {
        public NullShape(BinaryReader br):base(br)
        {

        }
    }
}
