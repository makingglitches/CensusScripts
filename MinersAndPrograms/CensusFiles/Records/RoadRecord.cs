using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DbfDataReader;
using ShapeUtilities;
using System.IO;

namespace CensusFiles
{
    public class RoadRecord : RoadBase,IRecordLoader,IHasShape
    {

        public string StCtyLinId { get; set; }
        public string FipsId { get; set; }
        
        public BaseShapeRecord Shape { get; set; }

        public void PutRecord(DataTable tgt)
        {
            DataRow dr =  tgt.NewRow();

            dr["StCtyLinId"] = this.StCtyLinId;
            dr["LinearId"]= this.LINEARID;
            dr["FullName"] = this.FULLNAME;
            dr["RouteType"] = this.RTTYP;
            dr["MafFeatureCode"] = this.MTFCC;
            dr["Shape"] = Shape.GetMSSQLInstance();
            dr["FipsId"] = FipsId;
            var bounding = Shape?.GetExtent();

            if (bounding != null)
            {
                dr["MinLongitude"] = bounding.X1;
                dr["MinLatitude"] = bounding.Y1;
                dr["MaxLongitude"] = bounding.X2;
                dr["MaxLatitude"] = bounding.Y2;
            }

            tgt.Rows.Add(dr);
        }
    }
}