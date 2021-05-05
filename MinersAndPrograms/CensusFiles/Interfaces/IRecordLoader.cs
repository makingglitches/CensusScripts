using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace CensusFiles
{
   public interface IRecordLoader:IBaseReader
    {
        void PutRecord( DataTable tgt);
    }
}
