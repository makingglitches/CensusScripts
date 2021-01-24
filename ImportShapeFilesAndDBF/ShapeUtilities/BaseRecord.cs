﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DbfDataReader;


// useful as previous but not specifcally what we're looking for here.
// apparently a whole geometric model with all coordinates and metadata can be defined in wkt
// this doesnt have what we're looking for otherwise however.
// http://docs.opengeospatial.org/is/18-010r7/18-010r7.html#1

namespace ShapeUtilities
{
   public class BaseRecord
    {

        public BaseRecord(BinaryReader reader)
        {
          
        }

        public string GetWKT()
        {

            StringBuilder sb = new StringBuilder();


            if (this is MultiPatchShape)
            {
                var obj = (MultiPatchShape)this;
                sb.Append("MULTIPATCH(");
            }

            if (this is MultiPointMShape)
            {
                var obj = (MultiPointMShape)this;
                sb.Append("MULTIPOINT(");
            }

            if (this is MultiPointShape)
            {
                var obj = (MultiPointShape)this;
                sb.Append("MULTIPOINT(");
            }

            if (this is MultiPointZShape)
            {
                var obj = (MultiPointZShape)this;
                sb.Append("MULTIPOINT(");
            }

            if (this is NullShape)
            {
                return String.Empty;
            }

            if (this is PointMShape)
            {
                var obj = (PointMShape)this;
                sb.Append("POINT(");
            }

            if (this is PointShape)
            {
                var obj = (PointShape)this;
                sb.Append("POINT(");
            }

            if (this is PointZShape)
            {
                var obj = (PointZShape)this;
                sb.Append("POINT(");
            }

            if (this is PolygonMShape)
            {
                var obj = (PolygonMShape)this;
                sb.Append("POLYGON(");
            }

            if (this is PolygonShape)
            {
                var obj = (PolygonShape)this;
                sb.Append("POLYGON(");
            }

            if (this is PolygonZShape)
            {
                var obj = (PolygonZShape)this;
                sb.Append("POLYGON(");
            }

            if (this is PolyLineMShape)
            {
                var obj = (PolyLineMShape)this;
                sb.Append("POLYLINE(");
            }

            if (this is PolyLineShape)
            {
                var obj = (PolyLineShape)this;
                sb.Append("POLYLINE(");
            }

            if (this is PolyLineZShape)
            {
                var obj = (PolyLineZShape)this;
                sb.Append("POLYLINE(");
            }

            sb.Append(")");
            
            return string.Empty;
        }

        public static BaseRecord ReadRecord(BinaryReader reader)
        {
            return new NullShape(reader);
        }

        public List<ShpPoint> ReadPoints(BinaryReader br,  int numberpoints)
        {
            List<ShpPoint> retval = new List<ShpPoint>();

            for (int x = 0; x < numberpoints; x++)
            {
                retval.Add(new ShpPoint(br));
            }

            return retval;
        }

        public List<double> ReadDoubles(BinaryReader br, int num)
        {
            List<double> retval = new List<double>();

            for (int x=0; x< num;x++)
            {
                retval.Add(br.ReadDouble());
            }

            return retval;
        }

        public List<int> ReadParts(BinaryReader br, int numparts)
        {
            List<int> retval = new List<int>();

            for (int x = 0; x < numparts; x++)
            {
                retval.Add(br.ReadInt32());
            }

            return retval;
        }
    }
}
