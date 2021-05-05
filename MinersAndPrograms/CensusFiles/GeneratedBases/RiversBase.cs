
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
    public abstract class RiversBase : IBaseReader
    {
        public Nullable<Int32> OBJECTID { get; set; }
        public String Name { get; set; }
        public String Feature { get; set; }
        public String State { get; set; }
        public Nullable<Int32> Region { get; set; }
        public Nullable<Decimal> Miles { get; set; }
        public Nullable<Decimal> Shape__Len { get; set; }

        public void Read(DbDataReader dread)
        {
            this.OBJECTID = (Nullable<Int32>)dread["OBJECTID"];
            this.Name = (String)dread["Name"];
            this.Feature = (String)dread["Feature"];
            this.State = (String)dread["State"];
            this.Region = (Nullable<Int32>)dread["Region"];
            this.Miles = (Nullable<Decimal>)dread["Miles"];
            this.Shape__Len = (Nullable<Decimal>)dread["Shape__Len"];

        }
    }
}
