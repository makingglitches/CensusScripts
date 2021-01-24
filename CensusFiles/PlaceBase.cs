
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;

namespace CensusFiles
{
    public abstract class PlaceBase:IBaseReader
    {
        public String STATEFP { get; set; }
        public String PLACEFP { get; set; }
        public String PLACENS { get; set; }
        public String GEOID { get; set; }
        public String NAME { get; set; }
        public String NAMELSAD { get; set; }
        public String LSAD { get; set; }
        public String CLASSFP { get; set; }
        public String PCICBSA { get; set; }
        public String PCINECTA { get; set; }
        public String MTFCC { get; set; }
        public String FUNCSTAT { get; set; }
        public Nullable<Int64> ALAND { get; set; }
        public Nullable<Int64> AWATER { get; set; }
        public String INTPTLAT { get; set; }
        public String INTPTLON { get; set; }

        public void Read(DbfDataReader.DbfDataReader dread)
        {
            this.STATEFP = (String)dread["STATEFP"];
            this.PLACEFP = (String)dread["PLACEFP"];
            this.PLACENS = (String)dread["PLACENS"];
            this.GEOID = (String)dread["GEOID"];
            this.NAME = (String)dread["NAME"];
            this.NAMELSAD = (String)dread["NAMELSAD"];
            this.LSAD = (String)dread["LSAD"];
            this.CLASSFP = (String)dread["CLASSFP"];
            this.PCICBSA = (String)dread["PCICBSA"];
            this.PCINECTA = (String)dread["PCINECTA"];
            this.MTFCC = (String)dread["MTFCC"];
            this.FUNCSTAT = (String)dread["FUNCSTAT"];
            this.ALAND = (Nullable<Int64>)dread["ALAND"];
            this.AWATER = (Nullable<Int64>)dread["AWATER"];
            this.INTPTLAT = (String)dread["INTPTLAT"];
            this.INTPTLON = (String)dread["INTPTLON"];

        }
    }
}
