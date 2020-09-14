using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
    public class MultiPointShape:BaseRecord
    {
        public ShpBox BoundingBox { get; set; }
        public int NumberPoints { get; set; }

        public List<ShpPoint> Points { get; set; }
        public MultiPointShape(BinaryReader br):base(br)
        {

            BoundingBox = new ShpBox(br);
            NumberPoints = br.ReadInt32();

            Points = ReadPoints(br, NumberPoints);

        }
    }
}
