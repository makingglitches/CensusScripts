using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfDataReader;

namespace Fixfbfcrap
{
   public class RecordHeader
    {
        public int RecordNumber { get; set; }
        public int ContentLength { get; set; }

        public SHPHeader.eShapeType ShapeType { get; set; }


        public RecordHeader(BinaryReader br)
        {
            RecordNumber = br.ReadBigEndianInt32();

            ContentLength = br.ReadBigEndianInt32();

            ShapeType = (SHPHeader.eShapeType)br.ReadInt32();

            switch (ShapeType)
            {
                case SHPHeader.eShapeType.Point:
                    Record = new PointShape(br);
                    break;

                case SHPHeader.eShapeType.PointM:
                    Record = new PointMShape(br);
                    break;

                case SHPHeader.eShapeType.PointZ:
                    Record = new PointZShape(br);
                    break;

                case SHPHeader.eShapeType.Polygon:
                    Record = new PolygonShape(br);
                    break;

                case SHPHeader.eShapeType.PolygonM:
                    Record = new PolygonMShape(br);
                    break;

                case SHPHeader.eShapeType.PolygonZ:
                    Record = new PolygonZShape(br);
                    break;

                case SHPHeader.eShapeType.PolyLine:
                    Record = new PolyLineShape(br);
                    break;

                case SHPHeader.eShapeType.PolyLineM:
                    Record = new PolyLineMShape(br);
                    break;

                case SHPHeader.eShapeType.PolyLineZ:
                    Record = new PolyLineZShape(br);
                    break;
                 
                case SHPHeader.eShapeType.MultiPatch:
                    Record = new MultiPatchShape(br);
                    break;

                case SHPHeader.eShapeType.MultiPoint:
                    Record = new MultiPointShape(br);
                    break;

                case SHPHeader.eShapeType.MultiPointM:
                    Record = new MultiPointMShape(br);
                    break;

                case SHPHeader.eShapeType.MultiPointZ:
                    Record = new MultiPointZShape(br);
                    break;

                case SHPHeader.eShapeType.NullShape:
                    Record = new NullShape(br);
                    break;

                default:
                    throw new Exception("None of the registered shape types matched header input !");
            }
        }


        public BaseRecord Record;
    }
}
