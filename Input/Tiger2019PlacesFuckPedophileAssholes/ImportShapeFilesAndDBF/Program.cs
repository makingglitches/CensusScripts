using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;

namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {

            // this john s decided to add to avoid using crappy m$ methods.
            var dbfPath = @"C:\Users\John\Documents\QrCode\Input\Tiger2019PlacesFuckPedophileAssholes\tl_2019_01_place.dbf";

            using (var dbfTable = new DbfTable(dbfPath, Encoding.UTF8))
            {
                var header = dbfTable.Header;

                var versionDescription = header.VersionDescription;
                var hasMemo = dbfTable.Memo != null;
                var recordCount = header.RecordCount;

                foreach (var dbfColumn in dbfTable.Columns)
                {
                    var name = dbfColumn.Name;
                    var columnType = dbfColumn.ColumnType;
                    var length = dbfColumn.Length;
                    var decimalCount = dbfColumn.DecimalCount;
                }
            }


        }
    }
}
