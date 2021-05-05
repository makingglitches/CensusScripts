
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Data;
using DbfDataReader;

namespace CensusFiles
{
    public abstract class SpeciesSeasonBase : IBaseReader
    {
        public Nullable<Int32> SeasonCode { get; set; }
        public String SeasonName { get; set; }

        public void Read(DbDataReader dread)
        {
            this.SeasonCode = (Nullable<Int32>)dread["SeasonCode"];
            this.SeasonName = (String)dread["SeasonName"];

        }
    }
}
