
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace CensusFiles
{
    public abstract class CensusBase
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
    }
}
