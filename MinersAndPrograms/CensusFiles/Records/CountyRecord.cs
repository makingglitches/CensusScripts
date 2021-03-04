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
    public class CountyRecord:CountyBase
    {
        public string FipsId { get; set; }
        public PolygonShape ShapeInfo { get; set; }

        public static event Action<CountyRecord> OnParse;
        public static event Action<long> OnFileLength;


        public static SqlCommand GetInsert(SqlConnection scon)
        {

            string cmd = File.ReadAllText("Queries\\InsertCounty.txt");

            SqlCommand insertcmd = new SqlCommand(cmd,scon);


            insertcmd.Parameters.Add("@FipsId", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@GeoId", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@GNISId", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@NameLSAD", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@LSAD", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@ClassFP", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@MTFCC", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@AreaLand", System.Data.SqlDbType.BigInt);
            insertcmd.Parameters.Add("@AreaWater", System.Data.SqlDbType.BigInt);
            insertcmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float);
            insertcmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float);
            insertcmd.Parameters.Add("@Shape", System.Data.SqlDbType.NVarChar);

            insertcmd.Parameters.Add("@MinLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MinLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLat", SqlDbType.Float);

            return insertcmd;

        }

        public void MapParameters(SqlCommand insertcmd)
        {
            object fipser = string.IsNullOrEmpty(FipsId) ? DBNull.Value as object : this.FipsId as object;

            object geomstring =
         ShapeInfo == null ? DBNull.Value as object :
         //"geography::STGeomFromText('" + 
         ShapeInfo.GetWKT();
         //+ "',4122)";

            insertcmd.Parameters["@FipsId"].Value = fipser;
            insertcmd.Parameters["@GeoId"].Value = this.GEOID;
            insertcmd.Parameters["@GNISId"].Value = this.COUNTYNS;
            insertcmd.Parameters["@Name"].Value = this.NAME;
            insertcmd.Parameters["@NameLSAD"].Value = this.NAMELSAD;
            insertcmd.Parameters["@LSAD"].Value = this.LSAD;
            insertcmd.Parameters["@ClassFP"].Value = this.CLASSFP;
            insertcmd.Parameters["@MTFCC"].Value = this.MTFCC;
            insertcmd.Parameters["@AreaLand"].Value = this.ALAND;
            insertcmd.Parameters["@AreaWater"].Value = this.AWATER;
            insertcmd.Parameters["@Longitude"].Value = this.INTPTLON;
            insertcmd.Parameters["@Latitude"].Value = this.INTPTLAT;
            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? ShapeInfo.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;
        }


            public static List<CountyRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode=false)
        {
      

            ShapeFile shpfile = null;

            if (loadShapeFile)
            {

                string shapefilename = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".shp";
                shpfile = new ShapeFile(shapefilename);
                shpfile.Load();
            }

            DbfDataReaderOptions ops = new DbfDataReaderOptions()
            {
                SkipDeletedRecords = true
            };

            DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(filename, ops);

            List<CountyRecord> results = new List<CountyRecord>();

      
            int shpfileindex = 0;

            if (eventmode)
            {
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }
         
            while (dread.Read())
            {
                CountyRecord r = new CountyRecord();
                r.Read(dread);

                var fips = FipsKeyRecord.GetCountyByState(r.STATEFP, scon);


                var ctyfip =  fips.Where(o => o.State == r.STATEFP && o.County == r.COUNTYFP).Select(o=>o.FipsId).
                    FirstOrDefault();

                r.FipsId = ctyfip;

                if (shpfile != null)
                {
                    r.ShapeInfo = (PolygonShape)shpfile.Records[shpfileindex].Record;
                    shpfileindex++;
                }

                if (eventmode)
                {
                    OnParse(r);
                }
                else
                {
                    results.Add(r);
                }

                r = null;
                GC.AddMemoryPressure(200000000);
                GC.Collect();
                GC.WaitForFullGCComplete();
            }

            dread.Close();

            return results;

        }
    }
}
