using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfDataReader;

namespace ShapeUtilities
{
   public class SHPHeader
    {
        /// <summary>
        /// should be the value 9994
        /// </summary>
        public int FileCode { get; set; }

        /// <summary>
        /// Number of word values in the shape file, so bytes/2.
        /// </summary>
        public int FileLength { get; set; }


        public int Version { get; set; }

        public eShapeType ShapeType { get; set; }

        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }

        public double ZMin { get; set; }
        public double ZMax { get; set; }

        public double MMin { get; set; }
        public double MMax { get; set; }

        public enum eShapeType : int
        {
            NullShape = 0,
            Point = 1,
            PolyLine = 3,
            Polygon = 5,
            MultiPoint = 8,
            PointZ = 11,
            PolyLineZ = 13,
            PolygonZ = 15,
            MultiPointZ = 18,
            PointM = 21,
            PolyLineM = 23,
            PolygonM = 25,
            MultiPointM = 28,
            MultiPatch = 31
        }

        

        public SHPHeader(BinaryReader reader)
        {
            //i have no fucking idea why they go from network order to host order bytes
            //zimmerman of course always has his fucked up comments.
            FileCode = reader.ReadBigEndianInt32();

            for (int x=0; x< 5; x++) { reader.ReadInt32(); }

            FileLength = reader.ReadBigEndianInt32();

            Version = reader.ReadInt32();

            ShapeType = (eShapeType)(reader.ReadInt32());

            XMin = reader.ReadDouble();
            ZMin = reader.ReadDouble();
            XMax = reader.ReadDouble();
            YMax = reader.ReadDouble();
            ZMin = reader.ReadDouble();
            ZMax = reader.ReadDouble();
            MMin = reader.ReadDouble();
            MMax = reader.ReadDouble();

            //Console.WriteLine(reader.BaseStream.Position);
        }
    }
}
