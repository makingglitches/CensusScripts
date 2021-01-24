using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;

namespace CensusFiles
{
   public interface IBaseReader
    {
        void Read(DbfDataReader.DbfDataReader dr);
    }
}
