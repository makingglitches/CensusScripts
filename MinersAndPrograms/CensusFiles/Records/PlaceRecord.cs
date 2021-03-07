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
    public class PlaceRecord :PlaceBase,IRecordLoader
    {

        public string FipsId { get; set; }

        public PolygonShape Shape { get; set; }

        // somewhere along the line, fort collins became a haven for whores and chomos that raped me in the 1980s
        // let the fbi know, they don't give a shit
        // hashtag bombs, terrorist, still alive, blow up world trade center, notice you fucking assholes !
        //.. jesus
        public static SqlCommand GetInsert(SqlConnection scon)
        {
            string cmd = File.ReadAllText("Queries\\InsertPlace.txt");

            SqlCommand insertcmd =
              new SqlCommand(cmd, scon);

            insertcmd.Parameters.Add("@FipsId", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@GeoId", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@GNISCode", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Name", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@LegalName", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@LSADId", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FipsClass", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@MetroOrMicroIndicator", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@CensusPlace", SqlDbType.Bit);
            insertcmd.Parameters.Add("@IncorporatedPlace", SqlDbType.Bit);
            insertcmd.Parameters.Add("@AreaLand", SqlDbType.Float);
            insertcmd.Parameters.Add("@AreaWater", SqlDbType.Float);
            insertcmd.Parameters.Add("@Latitude", SqlDbType.Float);
            insertcmd.Parameters.Add("@Longitude", SqlDbType.Float);
            insertcmd.Parameters.Add("@Shape", System.Data.SqlDbType.NVarChar);

            insertcmd.Parameters.Add("@MinLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MinLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLat", SqlDbType.Float);



            return insertcmd;
        }

        public static List<string> MissingFips = new List<string>();

        public void MapParameters(SqlCommand insertcmd)
        { 
            object fipser = string.IsNullOrEmpty(FipsId) ? DBNull.Value as object : this.FipsId as object;

            insertcmd.Parameters["@FipsId"].Value = fipser;
            insertcmd.Parameters["@GeoId"].Value = this.GEOID;
            insertcmd.Parameters["@GNISCode"].Value = this.PLACENS;
            insertcmd.Parameters["@Name"].Value = this.NAME;
            insertcmd.Parameters["@LegalName"].Value = this.NAMELSAD;
            insertcmd.Parameters["@LSADId"].Value = this.LSAD;
            insertcmd.Parameters["@FipsClass"].Value = this.CLASSFP;
            insertcmd.Parameters["@MetroOrMicroIndicator"].Value = this.PCICBSA;

            bool isCDP =this.MTFCC.ToString() == "GS4210"; // GS4110 is the value for incorporated, should check if there are null values in ds

            insertcmd.Parameters["@CensusPlace"].Value = isCDP;
            insertcmd.Parameters["@IncorporatedPlace"].Value = !isCDP;

            insertcmd.Parameters["@AreaLand"].Value = float.Parse(this.ALAND.ToString());
            insertcmd.Parameters["@AreaWater"].Value = float.Parse(this.AWATER.ToString());
            insertcmd.Parameters["@Latitude"].Value = float.Parse(this.INTPTLAT.ToString().Replace("+", ""));
            insertcmd.Parameters["@Longitude"].Value = float.Parse(this.INTPTLON.ToString().Replace("+", ""));

            object geomstring = 
                Shape == null ? DBNull.Value as object: 
                //"geography::STGeomFromText('" + 
                Shape.GetWKT() //+ "',4122)"
                ;

            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? Shape.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;



        }

        public PlaceRecord()
        {

        }

        public static  event Action<PlaceRecord> OnParse;
        public static  event Action<long> OnFileLength;

        

      /// <summary>
      /// Parses an entire CensusDBF For places, if eventmode is selected, will return an empty recordset 
      /// To bypass the memory pressure error SQL Server throws during load, and allow implementing code to handle record processing
      /// One at a time.
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="scon"></param>
      /// <param name="loadShapeFile"></param>
      /// <param name="resetMissingFips"></param>
      /// <param name="eventmode"></param>
      /// <returns></returns>
        public static List<PlaceRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile=false, bool resetMissingFips=false, bool eventmode=false)
        {

            ShapeFile shpfile = null;

            if (loadShapeFile)
            {
                string shapefilename = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".shp"; ;
                shpfile = new ShapeFile(shapefilename);
                shpfile.Load();
            }

            if (resetMissingFips) MissingFips = new List<string>();

            string[] pieces = filename.Split('_');
            string statecode = pieces[2];

            var FipsRecords = FipsKeyRecord.GetPlaceByState(statecode,scon);

            DbfDataReaderOptions ops = new DbfDataReaderOptions()
            {
                SkipDeletedRecords = true
            };

            List<PlaceRecord> results = new List<PlaceRecord>();

            DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(filename,ops);

            int shpfileindex = 0;


            if (eventmode)
            {
                // apparently the record length is in fact published, indirectly.
                // we'll see if its right. should be there have been no parse errors in the header load code.
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }

        

            while (dread.Read())
            {
                PlaceRecord pr = new PlaceRecord();
                pr.Read(dread);

                pr.FipsId = FipsRecords.Where(o => o.PlaceCode == pr.PLACEFP && o.State == pr.STATEFP).Select(o => o.FipsId).FirstOrDefault();

                if (string.IsNullOrEmpty(pr.FipsId))
                {
                    MissingFips.Add(pr.STATEFP + "\t" + pr.PLACEFP + "\t" + pr.NAME);          
                }

                if (shpfile!=null)
                {
                    pr.Shape = (PolygonShape)shpfile.Records[shpfileindex].Record;
                    shpfileindex++;
                }

                if (eventmode)
                {
                    OnParse(pr);

                }
                else
                {
                    results.Add(pr);
                }

                pr = null;
                GC.AddMemoryPressure(200000000);
                GC.Collect();
                GC.WaitForFullGCComplete();
            }

            dread.Close();

            return results;
        }

        public void PutRecord(DataTable tgt)
        {
            throw new NotImplementedException();
        }
    }
}
