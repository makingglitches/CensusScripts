using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fixfbfcrap
{
    public class PolyLineShape:BaseRecord
    {
        public ShpBox BoundingBox { get; set; }
        public int NumParts { get; set; }
        public int NumPoints { get; set; }

        public List<int> Parts { get; set; }

        public List<ShpPoint> Points { get; set; }

        public PolyLineShape(BinaryReader br):base(br)
        {

            BoundingBox = new ShpBox(br);

            NumParts = br.ReadInt32();
            NumPoints = br.ReadInt32();

            Parts = ReadParts(br, NumParts);

            Points = this.ReadPoints(br, NumPoints);
        }
    }
}
