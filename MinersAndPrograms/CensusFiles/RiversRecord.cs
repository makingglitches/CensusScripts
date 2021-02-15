using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using DbfDataReader;

namespace CensusFiles
{
    public class RiversRecord : RiversBase
    {
        public PolyLineShape ShapeInfo { get; set; }

        public static SqlCommand GetInsert(SqlConnection scon)
        {
            string cmd = File.ReadAllText("Queries\\InsertRiver.txt");

            SqlCommand insertcmd =
              new SqlCommand(cmd, scon);

            insertcmd.Parameters.Add("@ObjectId", SqlDbType.Int);
            insertcmd.Parameters.Add("@Name", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@State", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Region", SqlDbType.Int);
            insertcmd.Parameters.Add("@Miles", SqlDbType.Float);
            insertcmd.Parameters.Add("@ShapeLength", SqlDbType.Float);
            insertcmd.Parameters.Add("@Shape", SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@MinLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MinLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLon", SqlDbType.Float);


            return insertcmd;


        }

        // over and goddamn over i reproduce the same shit and if i dont someone else does and takes the credit and they hide someone
        // who can't explain themselves at all
        // the year is NOT 2021 they just somehow have got enough pieces of shit to play along. over a very long time
        // using a rather involved yet still simple process of ensnaring either one poor sod or one evil dumbass over and over.
        // eventually noone will remember being real. which is to say, truthful yet not while being imperfect and not being a total monster like them
        // which represets the terminus of human evolution and its step backwards to monkeys.
        // similar depressing thought really, but far from untrue.

        public static event Action<RiversRecord> OnParse;
        public static event Action<long> OnFileLength;

        public void MapParameters(SqlCommand insertcmd)
        {
            insertcmd.Parameters["@ObjectId"].Value = this.OBJECTID;
            insertcmd.Parameters["@Name"].Value = this.Name;
            insertcmd.Parameters["@State"].Value = this.State;
            insertcmd.Parameters["@Region"].Value= this.Region;
            insertcmd.Parameters["@Miles"].Value = this.Miles;
            insertcmd.Parameters["@ShapeLength"].Value = this.Shape__Len;

            object geomstring =
              ShapeInfo == null ? DBNull.Value as object :
              //"geography::STGeomFromText('" + 
              ShapeInfo.GetWKT() //+ "',4122)"
              ;

            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? ShapeInfo.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;
        }


        public static List<RiversRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false)
        {

            ShapeFile shpfile = null;

            if (loadShapeFile)
            {
                string shapefilename = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".shp"; ;
                shpfile = new ShapeFile(shapefilename);
                shpfile.Load();
            }


            DbfDataReaderOptions ops = new DbfDataReaderOptions()
            {
                SkipDeletedRecords = true
            };

            List<RiversRecord> results = new List<RiversRecord>();

            DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(filename, ops);

            int shpfileindex = 0;


            if (eventmode)
            {
                // apparently the record length is in fact published, indirectly.
                // we'll see if its right. should be there have been no parse errors in the header load code.
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }



            while (dread.Read())
            {
                RiversRecord pr = new RiversRecord();
                pr.Read(dread);

                if (shpfile != null)
                {
                    pr.ShapeInfo = (PolyLineShape)shpfile.Records[shpfileindex].Record;
                    shpfileindex++;
                }

                if (eventmode)
                {
                    OnParse(pr);

                }
                else
                {
                    results.Add(pr);
                }

                pr = null;
                GC.AddMemoryPressure(200000000);
                GC.Collect();
                GC.WaitForFullGCComplete();
            }

            dread.Close();

            return results;
        }
    }
}
