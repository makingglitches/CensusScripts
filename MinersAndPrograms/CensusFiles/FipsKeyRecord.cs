using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CensusFiles
{
  public  class FipsKeyRecord:FipsKeyBase
    {

        public static List<FipsKeyRecord> GetStates(SqlConnection scon)
        {
            SqlCommand getfips = new SqlCommand(@"SELECT * FROM[Geography].[dbo].[FipsKeys] where placecode = '00000' and county = '000'", scon);

            var sr = getfips.ExecuteReader();

            List<FipsKeyRecord> results = new List<FipsKeyRecord>();

            while (sr.Read())
            {
                var f = new FipsKeyRecord();

                f.Read(sr);
                results.Add(f);
            }

            sr.Close();

            return results;
        }
        public static List<FipsKeyRecord> GetPlaceByState(string numericStateCode, SqlConnection scon)
        {
            SqlCommand getfips = new SqlCommand("select * from dbo.FipsKeys f where f.PlaceCode <> '00000' and state=@state", scon);
            getfips.Parameters.AddWithValue("@state", numericStateCode);

            var sr = getfips.ExecuteReader();

            List<FipsKeyRecord> results = new List<FipsKeyRecord>();

            while (sr.Read())
            {
                var f = new FipsKeyRecord();

                f.Read(sr);
                results.Add(f);
            }

            sr.Close();

            return results;
        }

        public static List<FipsKeyRecord> GetCountyByState(string numericStateCode,SqlConnection scon)
        {
            SqlCommand getfips = new SqlCommand("SELECT * FROM[Geography].[dbo].[FipsKeys] where County<>'000' and State=@state", scon);
            getfips.Parameters.AddWithValue("@state", numericStateCode);

            var sr = getfips.ExecuteReader();

            List<FipsKeyRecord> results = new List<FipsKeyRecord>();

            while (sr.Read())
            {
                var f = new FipsKeyRecord();
                f.Read(sr);

                results.Add(f);
            }

            sr.Close();

            return results;
        }
    }
}
