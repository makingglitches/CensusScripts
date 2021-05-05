using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using DbfDataReader;

namespace CensusFiles
{
    public class AquiferRecord : AquiferBase,IRecordLoader,IHasShape
    {
        public BaseShapeRecord Shape { get; set; }

        public void PutRecord(DataTable tgt)
        {
            DataRow dr = tgt.NewRow();
            dr["AquiferCode"] = this.AQ_CODE;
            dr["AquiferName"] = this.AQ_NAME;

            var bounds = this.Shape?.GetExtent();

            if (bounds != null)
            {
                dr["MaxLatitude"] = bounds.X2;
                dr["MaxLongitude"] = bounds.Y2;
                dr["MinLatitude"] = bounds.X1;
                dr["MinLongitude"] = bounds.Y1;
            }


            dr["ObjectId"] = this.OBJECTID;
            dr["RockName"] = this.ROCK_NAME;
            dr["RockType"] = this.ROCK_TYPE;
            dr["Shape"] = this.Shape?.GetMSSQLInstance();
            dr["ShapeArea"] = this.Shape_Area;
            dr["ShapeLength"] = this.Shape_Leng;

            tgt.Rows.Add(dr);

        }
    }
}
