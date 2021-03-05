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
    public class StateRecord : StateBase,IRecordLoader
    {
        public string FipsId { get; set; }

        public PolygonShape ShapeInfo { get; set; }

        public static event Action<StateRecord> OnParse;
        public static event Action<long> OnFileLength;

        public static List<StateRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false)
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

            List<StateRecord> results = new List<StateRecord>();

            var fips =
            FipsKeyRecord.GetStates(scon);

            int shpfileindex = 0;

            if (eventmode)
            {
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }

            while (dread.Read())
            {
                StateRecord r = new StateRecord();
                r.Read(dread);

                r.FipsId = fips.Where(o => o.State == r.STATEFP).Select(o => o.FipsId).FirstOrDefault();

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

        public static SqlCommand GetInsert(SqlConnection scon)
        {
            // insert into dbo.Roads(Shape)
            // Values(geography::STGeomFromText('LINESTRING(-122.5360 47.656, -122.343 47.656)', 4326))

            string cmd = File.ReadAllText("Queries\\InsertState.txt");

            SqlCommand insertcmd = new SqlCommand(cmd, scon);

            insertcmd.Parameters.Add("@RegionCode", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@DivisionCode", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FipsKey", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@GNISKey", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Abbreviation", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Name", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@LSAD", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@ALand", SqlDbType.Float);
            insertcmd.Parameters.Add("@Awater", SqlDbType.Float);
            insertcmd.Parameters.Add("@Longitude", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Latitude", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Shape", SqlDbType.NVarChar);

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

            insertcmd.Parameters["@RegionCode"].Value =this.REGION;
            insertcmd.Parameters["@DivisionCode"].Value=this.DIVISION;
            insertcmd.Parameters["@FipsKey"].Value=this.STATEFP;
            insertcmd.Parameters["@GNISKey"].Value=this.STATENS;
            insertcmd.Parameters["@Abbreviation"].Value=this.STUSPS;
            insertcmd.Parameters["@Name"].Value=this.NAME;
            insertcmd.Parameters["@LSAD"].Value=this.LSAD;
            insertcmd.Parameters["@ALand"].Value=this.ALAND;
            insertcmd.Parameters["@Awater"].Value=this.AWATER;
            insertcmd.Parameters["@Longitude"].Value=this.INTPTLON;
            insertcmd.Parameters["@Latitude"].Value=this.INTPTLAT;
            insertcmd.Parameters["@Shape"].Value=geomstring;

            var bounding = geomstring != DBNull.Value ? ShapeInfo.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;


        }
    }
}