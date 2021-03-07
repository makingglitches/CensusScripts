using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ShapeUtilities;
using System.IO;
using System.Data.SqlTypes;
using System.Data.Common;
using DbfDataReader;
using System.Data;

namespace CensusFiles
{
    public class CountyRecord:CountyBase,IRecordLoader,IHasShape
    {
        public string FipsId { get; set; }
        public BaseShapeRecord Shape { get; set; }

        public void PutRecord(DataTable tgt)
        {

            DataRow dr = tgt.NewRow();

            var bounds = this.Shape?.GetExtent();

            dr["AreaLand"] = this.ALAND;
            dr["AreaWater"] = this.AWATER;
            dr["ClassFP"] = this.CLASSFP;
            dr["FipsId"] = this.FipsId;
            dr["GeoId"] = this.GEOID;
            dr["GNISId"] = this.COUNTYNS;
            dr["Latititude"] = this.INTPTLAT;
            dr["Longitude"] = this.INTPTLON;
            dr["LSAD"] = this.LSAD;

            if (bounds != null)
            {
                dr["MaxLatitude"] = bounds.X2;
                dr["MaxLongitude"] = bounds.Y2;
                dr["MinLatitude"] = bounds.X1;
                dr["MinLongitude"] = bounds.Y1;
            }

            dr["MTFCC"] = this.MTFCC;
            dr["Name"] = this.NAME;
            dr["NameLSAD"] = this.NAMELSAD;
            dr["Shape"] = this.Shape?.GetMSSQLInstance();

            tgt.Rows.Add(dr);
        }
    }
}
