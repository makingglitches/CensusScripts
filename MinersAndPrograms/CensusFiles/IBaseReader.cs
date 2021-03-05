using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.Data.Common;
using System.Data;

namespace CensusFiles
{
   public interface IBaseReader
    {
        void Read(DbDataReader dr);
            
    }
}
