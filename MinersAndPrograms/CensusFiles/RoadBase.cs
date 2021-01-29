
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
    public abstract class RoadBase : IBaseReader
    {
        public String LINEARID { get; set; }
        public String FULLNAME { get; set; }
        public String RTTYP { get; set; }
        public String MTFCC { get; set; }

        public void Read(DbDataReader dread)
        {
            this.LINEARID = (String)dread["LINEARID"];
            this.FULLNAME = (String)dread["FULLNAME"];
            this.RTTYP = (String)dread["RTTYP"];
            this.MTFCC = (String)dread["MTFCC"];

        }
    }
}
