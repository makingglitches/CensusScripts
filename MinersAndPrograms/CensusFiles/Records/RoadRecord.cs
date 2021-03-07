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


        #region Superceded
        public static event Action<RoadRecord> OnParse;
        public static event Action<long> OnFileLength;

        public static List<RoadRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false)
        {
            string[] pieces = filename.Split('_');
            string stcountycode = pieces[2];

            string statecode = stcountycode.Substring(0, 2);
            string countycode = stcountycode.Substring(2, 3);

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

            List<RoadRecord> results = new List<RoadRecord>();

            string fipsid =
            FipsKeyRecord.GetCountyByState(statecode, scon).Where(o => o.State == statecode && o.County == countycode).Select(o => o.FipsId).FirstOrDefault();

            int shpfileindex = 0;

            if (eventmode)
            {
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }

            while (dread.Read())
            {
                RoadRecord r = new RoadRecord();
                r.Read(dread);

               

                if (shpfile != null)
                {
                    r.Shape = (PolyLineShape)shpfile.Records[shpfileindex].Record;
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

        public static SqlCommand GetInsert(SqlConnection scon)
        {
            // insert into dbo.Roads(Shape)
            // Values(geography::STGeomFromText('LINESTRING(-122.5360 47.656, -122.343 47.656)', 4326))

            string cmd = File.ReadAllText("Queries\\InsertRoad.txt");

            SqlCommand insertcmd = new SqlCommand(cmd, scon);

            insertcmd.Parameters.Add("@LinearId", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FullName", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@RouteType", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FeatureCode", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Shape", System.Data.SqlDbType.NVarChar);

            insertcmd.Parameters.Add("@MinLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MinLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLat", SqlDbType.Float);


            return insertcmd;
        }

        public void MapParameters(SqlCommand insertcmd)
        {
            
            object geomstring =
           Shape == null ? DBNull.Value as object :
           //"geography::STGeomFromText('" + 
           Shape.GetWKT();
            //+ "',4122)";

           

            insertcmd.Parameters["@LinearId"].Value = this.LINEARID;
            insertcmd.Parameters["@FullName"].Value = this.FULLNAME;
            insertcmd.Parameters["@RouteType"].Value = this.RTTYP;
            insertcmd.Parameters["@FeatureCode"].Value = this.MTFCC;
            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? Shape.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;


        }

        #endregion Superceded

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