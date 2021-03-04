
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
    public abstract class StateBase : IBaseReader
    {
        public String REGION { get; set; }
        public String DIVISION { get; set; }
        public String STATEFP { get; set; }
        public String STATENS { get; set; }
        public String GEOID { get; set; }
        public String STUSPS { get; set; }
        public String NAME { get; set; }
        public String LSAD { get; set; }
        public String MTFCC { get; set; }
        public String FUNCSTAT { get; set; }
        public Nullable<Int64> ALAND { get; set; }
        public Nullable<Int64> AWATER { get; set; }
        public String INTPTLAT { get; set; }
        public String INTPTLON { get; set; }

        public void Read(DbDataReader dread)
        {
            this.REGION = (String)dread["REGION"];
            this.DIVISION = (String)dread["DIVISION"];
            this.STATEFP = (String)dread["STATEFP"];
            this.STATENS = (String)dread["STATENS"];
            this.GEOID = (String)dread["GEOID"];
            this.STUSPS = (String)dread["STUSPS"];
            this.NAME = (String)dread["NAME"];
            this.LSAD = (String)dread["LSAD"];
            this.MTFCC = (String)dread["MTFCC"];
            this.FUNCSTAT = (String)dread["FUNCSTAT"];
            this.ALAND = (Nullable<Int64>)dread["ALAND"];
            this.AWATER = (Nullable<Int64>)dread["AWATER"];
            this.INTPTLAT = (String)dread["INTPTLAT"];
            this.INTPTLON = (String)dread["INTPTLON"];

        }
    }
}
