using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;
using Microsoft.SqlServer.Types;

namespace CensusFiles
{
    public class RiversRecord : RiversBase,IRecordLoader, IHasShape
    {        
        public BaseShapeRecord Shape { get; set; }

        public void PutRecord(DataTable dt)
        {

            DataRow dr = dt.NewRow();

            dr["ObjectId"] = this.OBJECTID;
            dr["Name"] = this.Name;
            dr["StateAbbreviation"] = this.State;
            dr["Region"] = this.Region;
            dr["Miles"] = this.Miles;
            dr["ShapeLength"] = this.Shape__Len;
            dr["Shape"] = this.Shape?.GetMSSQLInstance();

            var bounding = this.Shape?.GetExtent();

            if (bounding != null)
            {
                dr["MinLatitude"] = bounding.X1;
                dr["MinLongitude"] = bounding.Y1;
                dr["MaxLatitude"] = bounding.X2;
                dr["MaxLongitude"] = bounding.Y2;
            }

            dt.Rows.Add(dr);
        }

    }
}
