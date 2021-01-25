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
        public static List<FipsKeyRecord> GetByState(string numericStateCode, SqlConnection scon)
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

            return results;
        }
    }
}
