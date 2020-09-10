using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Data.Linq;


namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {

            // this john s decided to add to avoid using crappy m$ methods.
            // only software that interacts with their own software tends to be well written
            // as per their world corporate takeover apparently suck small penises fucking
            // business model.
            // that or the david pumpkins bullshit mimicking an actor these fucks venerate heh

            var dbfPath = @"C:\Users\John\Documents\QrCode\Input\Places2019\places";

            string[] dbasefiles = Directory.GetFiles(dbfPath, "*.dbf");

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            csb.DataSource = "localhost";
            csb.IntegratedSecurity = true;
            csb.InitialCatalog = "Geography";

            SqlConnection scon = new SqlConnection(csb.ConnectionString);

            SqlCommand getfips = new SqlCommand("select * from dbo.FipsKeys f where f.PlaceCode <> '00000' and state=@state", scon);
            getfips.Parameters.AddWithValue("@state", "01");

            SqlDataAdapter sd =
                     new SqlDataAdapter(getfips);

            // honestly this could be divided into loadable string data that got autoformatted by a helper
            // but dont see that much reusability occurring in this code so why do so ?
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
                                ,@Longitude)",scon);

            insertplace.Parameters.Add("@FipsId", SqlDbType.Int);
            insertplace.Parameters.Add("@GeoId", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@GNISCode", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@Name", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@LegalName", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@LSADId", SqlDbType.Int);
            insertplace.Parameters.Add("@FipsClass", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@MetroOrMicroIndicator", SqlDbType.NVarChar);
            insertplace.Parameters.Add("@CensusPlace", SqlDbType.Bit);
            insertplace.Parameters.Add("@IncorporatedPlace", SqlDbType.Bit);
            insertplace.Parameters.Add("@AreaLand", SqlDbType.Float);
            insertplace.Parameters.Add("@AreaWater", SqlDbType.Float);
            insertplace.Parameters.Add("@Latitude", SqlDbType.Float);
            insertplace.Parameters.Add("@Longitude", SqlDbType.Float);


            scon.Open();


            // loop through each place data dbf file
            foreach (string dbfname in dbasefiles)
            {
                string[] pieces = dbfname.Split('_');
                string statecode = pieces[2];

                DbfDataReader.DbfDataReader dr = new DbfDataReader.DbfDataReader(dbfname);

                getfips.Parameters["@state"].Value = statecode;

                DataSet ds = new DataSet();
                sd.Fill(ds);

                var BETTERFIPPER =
                    ds.Tables[0].AsEnumerable().
                    Select(o => new { FipsId = (int)o["FipsId"], State = o["State"], Place = o["PlaceCode"] }).ToList();

                while (dr.Read())
                {

                    int fipsid = BETTERFIPPER.
                     Where(o => o.State.Equals(dr["STATEFP"]) &&
                    o.Place.Equals(dr["PLACEFP"])).Select(o => o.FipsId).First();



                    insertplace.Parameters["@FipsId"].Value = fipsid;
                    insertplace.Parameters["@GeoId"].Value = dr["GEOID"];
                    insertplace.Parameters["@GNISCode"].Value = dr["PLACENS"];
                    insertplace.Parameters["@Name"].Value = dr["NAME"];
                    insertplace.Parameters["@LegalName"].Value = dr["NAMELSAD"];
                    insertplace.Parameters["@LSADId"].Value = int.Parse(dr["LSAD"].ToString());
                    insertplace.Parameters["@FipsClass"].Value = dr["CLASSFP"];
                    insertplace.Parameters["@MetroOrMicroIndicator"].Value = dr["PCICBSA"];

                    bool isCDP = dr["MTFCC"].ToString() == "GS4210"; // GS4110 is the value for incorporated, should check if there are null values in ds

                    insertplace.Parameters["@CensusPlace"].Value = isCDP;
                    insertplace.Parameters["@IncorporatedPlace"].Value = !isCDP;

                    insertplace.Parameters["@AreaLand"].Value = float.Parse(dr["ALAND"].ToString());
                    insertplace.Parameters["@AreaWater"].Value = float.Parse(dr["AWATER"].ToString());
                    insertplace.Parameters["@Latitude"].Value = float.Parse(dr["INTPTLAT"].ToString().Replace("+", ""));
                    insertplace.Parameters["@Longitude"].Value = float.Parse(dr["INTPTLON"].ToString().Replace("+", ""));

                    if (insertplace.ExecuteNonQuery() <= 0) throw new Exception("something fucked up place didnt get inserted");
                }


                dr.Close();

            }

            scon.Close();
        }
    }
}
