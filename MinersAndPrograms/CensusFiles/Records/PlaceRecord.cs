using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;
using ShapeUtilities;
using System.IO;

namespace CensusFiles
{
    public class PlaceRecord :PlaceBase,IRecordLoader,IHasShape
    {

        public PlaceRecord()
        {

        }

        public string FipsId { get; set; }

        public BaseShapeRecord Shape { get; set; }

        public void PutRecord(DataTable tgt)
        {
            DataRow dr = tgt.NewRow();

            dr["AreaLand"] = this.ALAND;
            dr["AreaWater"] = this.AWATER;

            dr["FipsClass"] = this.CLASSFP;
            dr["FipsId"] = this.FipsId;
            dr["GeoId"] = this.GEOID;
            dr["GNISCode"] = this.PLACENS;


            
            dr["Latitude"] = this.INTPTLAT;
            dr["LegalName"] = this.NAMELSAD;
            dr["Longitude"] = this.INTPTLON;
            dr["LSADId"] = this.LSAD;


            bool isCDP = this.MTFCC.ToString() == "GS4210"; // GS4110 is the value for incorporated, should check if there are null values in ds

            dr["CensusPlace"] = isCDP;
            dr["IncorporatedPlace"] = !isCDP;



            dr["MetroOrMicroIndicator"] = this.PCICBSA;


            dr["Name"] = this.NAME;

            dr["Shape"] = this.Shape?.GetMSSQLInstance();

            var bounding = this.Shape?.GetExtent();

            if (bounding != null)
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
