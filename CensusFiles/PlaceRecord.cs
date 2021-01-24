using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace CensusFiles
{
    public class PlaceRecord :PlaceBase
    {
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

        public PlaceRecord()
        {

        }

      
    }
}
