using System;
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

        /// <summary>
        /// Gets the bounding box of a shape implemeting IHasPoints
        /// </summary>
        /// <returns>Null if shape does not a have a points collection otherwise returns a boundingbox enclosing the points</returns>
        public BoundingBox GetExtent()
        {
            if (this is IHasPoints)
            {
                var obj = (IHasPoints)this;

                var xes = obj.Points.Select(o => o.X).ToList();
                var yes = obj.Points.Select(o => o.Y).ToList();

                return new BoundingBox() { X1 = xes.Min(), X2 = xes.Max(), Y1 = yes.Min(), Y2 = yes.Max() };
            }
            else return null;
          
        }


        public string GetWKT()
        {

            StringBuilder sb = new StringBuilder();

            #region Implemented

            if (this is PolygonShape)
            {
                var obj = (PolygonShape)this;
                sb.Append("POLYGON(");



                for (int parts = 0; parts < obj.NumParts; parts++)
                {
                    sb.Append("(");

                    int index = obj.Parts[parts];

                    int lastindex = 0;

                    if (obj.NumParts==1)
                    {
                        lastindex = obj.NumPoints - 1;   
                    }
                    else if (parts < obj.NumParts-1)
                    {
                        lastindex = obj.Parts[parts + 1] - 1;
                    }
                    else
                    {
                        lastindex = obj.NumPoints - 1;
                    }
                    
                    for (int pointindex = index; pointindex < lastindex + 1; pointindex++)
                    {
                        sb.Append(obj.Points[pointindex].X.ToString() + " " +
                            obj.Points[pointindex].Y.ToString() + 
                            (pointindex==lastindex?" ": ","));

                    }

                    sb.Append(")");

                    if (parts < obj.NumParts - 1)
                    {
                        sb.Append(", ");
                    }

                }

                sb.Append(")");

                return sb.ToString();
            }

            if (this is PolyLineShape)
            {
                var obj = (PolyLineShape)this;
                sb.Append("LINESTRING(");

                for (int parts = 0; parts < obj.NumParts; parts++)
                {
                    sb.Append("(");

                    int index = obj.Parts[parts];

                    int lastindex = 0;

                    if (obj.NumParts == 1)
                    {
                        lastindex = obj.NumPoints - 1;
                    }
                    else if (parts < obj.NumParts - 1)
                    {
                        lastindex = obj.Parts[parts + 1] - 1;
                    }
                    else
                    {
                        lastindex = obj.NumPoints - 1;
                    }

                    for (int pointindex = index; pointindex < lastindex + 1; pointindex++)
                    {
                        sb.Append(obj.Points[pointindex].X.ToString() + " " +
                            obj.Points[pointindex].Y.ToString() +
                            (pointindex == lastindex ? " " : ","));

                    }

                    sb.Append(")");

                    if (parts < obj.NumParts - 1)
                    {
                        sb.Append(", ");
                    }

                }

                sb.Append(")");

                return sb.ToString();

            }

            #endregion

            #region NotImplemented
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

            if (this is PolyLineZShape)
            {
                var obj = (PolyLineZShape)this;
                sb.Append("POLYLINE(");
            }

            #endregion

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
