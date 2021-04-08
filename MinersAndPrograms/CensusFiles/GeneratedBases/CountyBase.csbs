
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
    public abstract class CountyBase : IBaseReader
    {
        public String STATEFP { get; set; }
        public String COUNTYFP { get; set; }
        public String COUNTYNS { get; set; }
        public String GEOID { get; set; }
        public String NAME { get; set; }
        public String NAMELSAD { get; set; }
        public String LSAD { get; set; }
        public String CLASSFP { get; set; }
        public String MTFCC { get; set; }
        public String CSAFP { get; set; }
        public String CBSAFP { get; set; }
        public String METDIVFP { get; set; }
        public String FUNCSTAT { get; set; }
        public Nullable<Int64> ALAND { get; set; }
        public Nullable<Int64> AWATER { get; set; }
        public String INTPTLAT { get; set; }
        public String INTPTLON { get; set; }

        public void Read(DbDataReader dread)
        {
            this.STATEFP = (String)dread["STATEFP"];
            this.COUNTYFP = (String)dread["COUNTYFP"];
            this.COUNTYNS = (String)dread["COUNTYNS"];
            this.GEOID = (String)dread["GEOID"];
            this.NAME = (String)dread["NAME"];
            this.NAMELSAD = (String)dread["NAMELSAD"];
            this.LSAD = (String)dread["LSAD"];
            this.CLASSFP = (String)dread["CLASSFP"];
            this.MTFCC = (String)dread["MTFCC"];
            this.CSAFP = (String)dread["CSAFP"];
            this.CBSAFP = (String)dread["CBSAFP"];
            this.METDIVFP = (String)dread["METDIVFP"];
            this.FUNCSTAT = (String)dread["FUNCSTAT"];
            this.ALAND = (Nullable<Int64>)dread["ALAND"];
            this.AWATER = (Nullable<Int64>)dread["AWATER"];
            this.INTPTLAT = (String)dread["INTPTLAT"];
            this.INTPTLON = (String)dread["INTPTLON"];

        }
    }
}
