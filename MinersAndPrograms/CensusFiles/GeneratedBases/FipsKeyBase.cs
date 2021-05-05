
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
    public abstract class FipsKeyBase : IBaseReader
    {
        public String SummLevel { get; set; }
        public String State { get; set; }
        public String County { get; set; }
        public String CountyDivision { get; set; }
        public String PlaceCode { get; set; }
        public String ConsolidatedCity { get; set; }
        public String AreaName { get; set; }
        public String FipsId { get; set; }

        public void Read(DbDataReader dread)
        {
            this.SummLevel = (String)dread["SummLevel"];
            this.State = (String)dread["State"];
            this.County = (String)dread["County"];
            this.CountyDivision = (String)dread["CountyDivision"];
            this.PlaceCode = (String)dread["PlaceCode"];
            this.ConsolidatedCity = (String)dread["ConsolidatedCity"];
            this.AreaName = (String)dread["AreaName"];
            this.FipsId = (String)dread["FipsId"];

        }
    }
}
