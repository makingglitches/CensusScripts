using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;

namespace CensusFiles
{
    public class SpeciesSeasonRecord : SpeciesSeasonBase, IRecordLoader, IHasShape
    {
        public BaseShapeRecord Shape { get; set; }
        public string DownloadGuid { get; set; }

        public void PutRecord(DataTable tgt)
        {
            DataRow dr = tgt.NewRow();
            dr["DownloadGuid"] = this.DownloadGuid;
            dr["Season"] = this.SeasonCode;
            dr["SeasonName"] = this.SeasonName;
            dr["Shape"] = this.Shape?.GetMSSQLInstance();
            
            var bounding = this.Shape?.GetExtent(); if (bounding != null) 
            { 
                dr["MinLatitude"] = bounding.X1; 
                dr["MinLongitude"] = bounding.Y1; 
                dr["MaxLatitude"] = bounding.X2; 
                dr["MaxLongitude"] = bounding.Y2; 
            }

            tgt.Rows.Add(dr);
        }

    }
}
