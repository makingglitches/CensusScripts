using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;

namespace CensusFiles
{
    public class PlaceRecord :PlaceBase
    {

        public string FipsId { get; set; }

        public static SqlCommand GetInsert(SqlConnection scon)
        {
            SqlCommand insertplace =
              new SqlCommand(@"INSERT INTO[dbo].[Places]
                                ([FipsId]
                                ,[GeoId]
                                ,[GNISCode]
                                ,[Name]
                                ,[LegalName]
                                ,[LSADId]
                                ,[FipsClass]
                                ,[MetroOrMicroIndicator]
                                ,[CensusPlace]
                                ,[IncorporatedPlace]
                                ,[AreaLand]
                                ,[AreaWater]
                                ,[Latitude]
                                ,[Longitude])
                            VALUES
                                (@FipsId
                                ,@GeoId
                                ,@GNISCode
                                ,@Name
                                ,@LegalName
                                ,@LSADId
                                ,@FipsClass
                                ,@MetroOrMicroIndicator
                                ,@CensusPlace
                                ,@IncorporatedPlace
                                ,@AreaLand
                                ,@AreaWater
                                ,@Latitude
                                ,@Longitude)", scon);

            insertplace.Parameters.Add("@FipsId", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@GeoId", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@GNISCode", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@Name", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@LegalName", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@LSADId", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@FipsClass", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@MetroOrMicroIndicator", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@CensusPlace", SqlDbType.Bit);
            insertplace.Parameters.Add("@IncorporatedPlace", SqlDbType.Bit);
            insertplace.Parameters.Add("@AreaLand", SqlDbType.Float);
            insertplace.Parameters.Add("@AreaWater", SqlDbType.Float);
            insertplace.Parameters.Add("@Latitude", SqlDbType.Float);
            insertplace.Parameters.Add("@Longitude", SqlDbType.Float);

           

            return insertplace;
        }

        public static List<string> MissingFips = new List<string>();

        public void MapParameters(SqlCommand insertPlace, List<FipsKeyRecord> fips)
        { 
            object fipser = string.IsNullOrEmpty(FipsId) ? DBNull.Value as object : this.FipsId as object;

            insertPlace.Parameters["@FipsId"].Value = fipser;
            insertPlace.Parameters["@GeoId"].Value = this.GEOID;
            insertPlace.Parameters["@GNISCode"].Value = this.PLACENS;
            insertPlace.Parameters["@Name"].Value = this.NAME;
            insertPlace.Parameters["@LegalName"].Value = this.NAMELSAD;
            insertPlace.Parameters["@LSADId"].Value = this.LSAD;
            insertPlace.Parameters["@FipsClass"].Value = this.CLASSFP;
            insertPlace.Parameters["@MetroOrMicroIndicator"].Value = this.PCICBSA;

            bool isCDP =this.MTFCC.ToString() == "GS4210"; // GS4110 is the value for incorporated, should check if there are null values in ds

            insertPlace.Parameters["@CensusPlace"].Value = isCDP;
            insertPlace.Parameters["@IncorporatedPlace"].Value = !isCDP;

            insertPlace.Parameters["@AreaLand"].Value = float.Parse(this.ALAND.ToString());
            insertPlace.Parameters["@AreaWater"].Value = float.Parse(this.AWATER.ToString());
            insertPlace.Parameters["@Latitude"].Value = float.Parse(this.INTPTLAT.ToString().Replace("+", ""));
            insertPlace.Parameters["@Longitude"].Value = float.Parse(this.INTPTLON.ToString().Replace("+", ""));
        }

        public PlaceRecord()
        {

        }

        /// <summary>
        /// Parses an entire censuus dbf file for places, could be altered to be generic for roads etc
        /// Also matches existing populated fips data 
        /// </summary>
        /// <param name="filename">absolute path to .dbf file</param>
        /// <param name="scon">an open sqlconnection</param>
        /// <param name="resetMissingFips">default false, but set to true to clear missingfips static list</param>
        /// <returns></returns>
        public static List<PlaceRecord> ParseDBFFile(string filename,SqlConnection scon, bool resetMissingFips=false)
        {
            if (resetMissingFips) MissingFips = new List<string>();

            string[] pieces = filename.Split('_');
            string statecode = pieces[2];

            var FipsRecords = FipsKeyRecord.GetByState(statecode,scon);

            DbfDataReaderOptions ops = new DbfDataReaderOptions()
            {
                SkipDeletedRecords = true
            };

            List<PlaceRecord> results = new List<PlaceRecord>();

            DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(filename,ops);

            while (dread.Read())
            {
                PlaceRecord pr = new PlaceRecord();
                pr.Read(dread);

                pr.FipsId = FipsRecords.Where(o => o.PlaceCode == pr.PLACEFP && o.State == pr.STATEFP).Select(o => o.FipsId).FirstOrDefault();

                if (string.IsNullOrEmpty(pr.FipsId))
                {
                    MissingFips.Add(pr.STATEFP + "\t" + pr.PLACEFP + "\t" + pr.NAME);          
                }

                results.Add(pr);
            }

            return results;
        }

      
    }
}
