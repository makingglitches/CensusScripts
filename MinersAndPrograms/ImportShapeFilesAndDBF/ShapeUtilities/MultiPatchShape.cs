using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShapeUtilities
{
    public enum PartType:int
    {
        TriangleStrip=0,
        TriangleFan=1,
        OuterRing=2,
        InnerRing=3,
        FirstRing=4,
        Ring=5

    }

   
    public class MultiPatchShape:BaseShapeRecord, IHasPoints
    {
        public ShpBox BoundingBox { get; set; }
        public int NumberParts { get; set; }
        public int NumberPoints { get; set; }

        public List<int> Parts { get; set; }

        public List<PartType> PartTypes { get; set; }

        public List<ShpPoint> Points { get; set; }

        public double ZMin { get; set; }
        public double ZMax { get; set; }

        public List<double> ZArray { get; set;}

        public double MMin { get; set; }
        public double MMax { get; set; }

        public List<double> MArray { get; set; }


        public MultiPatchShape(BinaryReader br):base(br)
        {
            BoundingBox = new ShpBox(br);
            NumberParts = br.ReadInt32();
            NumberPoints = br.ReadInt32();
            Parts = ReadParts(br, NumberParts);
            PartTypes = ReadParts(br, NumberParts).Select(o => (PartType)o).ToList();
            Points = ReadPoints(br, NumberPoints);
            ZMin = br.ReadDouble();
            ZMax = br.ReadDouble();
            ZArray = ReadDoubles (br, NumberPoints);
            MMin = br.ReadDouble();
            MMax = br.ReadDouble();
            MArray = ReadDoubles(br, NumberPoints);
        }
    }
}
