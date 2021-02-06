using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CensusFiles
{
    // for later questionably beneficial conversion to a more standardized form
    public interface IRecordWriter
    {
        void MapParameters(SqlCommand insertPlace);
        List<IBaseReader> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false);


    }
}
