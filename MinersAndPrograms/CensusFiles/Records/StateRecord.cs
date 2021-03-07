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
    public class StateRecord : StateBase, IRecordLoader, IHasShape
    {
        // tori needs to stop trying to insert stupid like 'only load and check one record at a time' bullshit to add anxiety to coding methods
        // i already know how to do bitch
        // fucking weird cunts
        // speaking of course of the resume functionality.
        // soooo i forgot that there were 245,000 records in the roads db
        // sooo i forgot there was a 2100 limit to parameters in sqlcommands so multi inserts wouldnt do
        // soooo i never really had much use for sqlbulkcopy because i was used to just bulk loading csv's for large data ops and/or
        // using builtin backup/restore functionality
        // i'm still experienced enough to know that goddamn checking one fucking id at a time and forging a seperate connection per is a ridiculous
        // waste of time and would be slower than shit !
        public string FipsId { get; set; }

        public BaseShapeRecord Shape { get; set; }

        #region Superceded
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
                    r.Shape = (PolygonShape)shpfile.Records[shpfileindex].Record;
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
           Shape == null ? DBNull.Value as object :
           //"geography::STGeomFromText('" + 
           Shape.GetWKT();
            //+ "',4122)";

            insertcmd.Parameters["@RegionCode"].Value = this.REGION;
            insertcmd.Parameters["@DivisionCode"].Value = this.DIVISION;
            insertcmd.Parameters["@FipsKey"].Value = this.STATEFP;
            insertcmd.Parameters["@GNISKey"].Value = this.STATENS;
            insertcmd.Parameters["@Abbreviation"].Value = this.STUSPS;
            insertcmd.Parameters["@Name"].Value = this.NAME;
            insertcmd.Parameters["@LSAD"].Value = this.LSAD;
            insertcmd.Parameters["@ALand"].Value = this.ALAND;
            insertcmd.Parameters["@Awater"].Value = this.AWATER;
            insertcmd.Parameters["@Longitude"].Value = this.INTPTLON;
            insertcmd.Parameters["@Latitude"].Value = this.INTPTLAT;
            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? Shape.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;


        }

        #endregion Superceded

        // besides having said that before this is mostly about burying hateful anxiety causing text in a github database
        // though i want to keep this goddamn code and not have some child molester from colorado or elsewhere or routed through
        // colorado steal it or my identity since they tried their best to ensure i couldnt get back on my feet over and over again
        // when i was being honest which is why they assault me and call the cops first nowadays to limit my travel and escape or progression
        // options just in time for them to roll the date back. fucking assholes.

        public void PutRecord(DataTable tgt)
        {
            DataRow dr = tgt.NewRow();
            dr["Abbreviation"] = this.STUSPS;
            dr["AreaLand"] = this.ALAND;
            dr["AreaWater"] = this.AWATER;
            dr["DivisionCode"] = this.DIVISION;
            dr["FipsKey"] = this.FipsId;
            dr["GNISKey"] = this.STATENS;
            dr["Latitude"] = this.INTPTLAT;
            dr["Longitude"] = this.INTPTLON;
            dr["LSAD"] = this.LSAD;
            dr["Name"] = this.NAME;
            dr["RegionCode"] = this.REGION;
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