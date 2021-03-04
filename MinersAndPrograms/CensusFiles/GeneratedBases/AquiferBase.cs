
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
    public abstract class AquiferBase : IBaseReader
    {
        public Nullable<Int32> OBJECTID { get; set; }
        public String ROCK_NAME { get; set; }
        public Nullable<Int32> ROCK_TYPE { get; set; }
        public String AQ_NAME { get; set; }
        public Nullable<Int32> AQ_CODE { get; set; }
        public Nullable<Single> Shape_Leng { get; set; }
        public Nullable<Single> Shape_Area { get; set; }

        public void Read(DbDataReader dread)
        {
            this.OBJECTID = (Nullable<Int32>)dread["OBJECTID"];
            this.ROCK_NAME = (String)dread["ROCK_NAME"];
            this.ROCK_TYPE = (Nullable<Int32>)dread["ROCK_TYPE"];
            this.AQ_NAME = (String)dread["AQ_NAME"];
            this.AQ_CODE = (Nullable<Int32>)dread["AQ_CODE"];
            this.Shape_Leng = (Nullable<Single>)dread["Shape_Leng"];
            this.Shape_Area = (Nullable<Single>)dread["Shape_Area"];

        }
    }
}
