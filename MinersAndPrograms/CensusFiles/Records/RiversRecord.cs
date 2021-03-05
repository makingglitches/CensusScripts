using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;
using Microsoft.SqlServer.Types;

namespace CensusFiles
{
    public class RiversRecord : RiversBase,IRecordLoader
    {        
        public PolyLineShape ShapeInfo { get; set; }

        public void PutRecord(DataTable dt)
        {

            DataRow dr = dt.NewRow();

            dr["ObjectId"] = this.OBJECTID;
            dr["Name"] = this.Name;
            dr["StateAbbreviation"] = this.State;
            dr["Region"] = this.Region;
            dr["Miles"] = this.Miles;
            dr["ShapeLength"] = this.Shape__Len;
            dr["Shape"] = this.ShapeInfo?.GetMSSQLInstance();

            var bounding = this.ShapeInfo?.GetExtent();

            if (bounding != null)
            {
                dr["MinLatitude"] = bounding.X1;
                dr["MinLongitude"] = bounding.Y1;
                dr["MaxLatitude"] = bounding.X2;
                dr["MaxLongitude"] = bounding.Y2;
            }

            dt.Rows.Add(dr);
        }

        #region SuperCeded
        public static event Action<RiversRecord> OnParse;
        public static event Action<long> OnFileLength;
        public static event Action<RiversRecord> SkipRecord;

        #region StandardInserts

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

        public void MapParameters(SqlCommand insertcmd, int row=0)
        {
            
            // this i already did having not really wanted to bother with datatable and sqlbulk copy.
            // actually a fairly easy idea really.
            // see the issue is that there is record type specific logic in some of these.
            insertcmd.Parameters["@ObjectId"+(row==0?"":row.ToString())].Value = this.OBJECTID;
            insertcmd.Parameters["@Name" + (row == 0 ? "" : row.ToString())].Value = this.Name;
            insertcmd.Parameters["@State" + (row == 0 ? "" : row.ToString())].Value = this.State.Trim();
            insertcmd.Parameters["@Region" + (row == 0 ? "" : row.ToString())].Value= this.Region;
            insertcmd.Parameters["@Miles" + (row == 0 ? "" : row.ToString())].Value = this.Miles;
            insertcmd.Parameters["@ShapeLength" + (row == 0 ? "" : row.ToString())].Value = this.Shape__Len;

            object geomstring =
              ShapeInfo == null ? DBNull.Value as object :
              //"geography::STGeomFromText('" + 
              ShapeInfo.GetWKT() //+ "',4122)"
              ;

            insertcmd.Parameters["@Shape" + (row == 0 ? "" : row.ToString())].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? ShapeInfo.GetExtent() : null;

            insertcmd.Parameters["@MinLon" + (row == 0 ? "" : row.ToString())].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat" + (row == 0 ? "" : row.ToString())].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon" + (row == 0 ? "" : row.ToString())].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat" + (row == 0 ? "" : row.ToString())].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;
        }


        public static SqlCommand InsertMultiple(List<RiversRecord> input, SqlConnection scon)
        {
            var ins = RiversRecord.GetInsert(scon);

            var s = ins.CommandText;

            int startv = s.IndexOf("VALUES");

            s = s.Substring(s.IndexOf("VALUES")+6);
            s = s.Replace("(", "");
            s = s.Replace(")", "");

            // just in case the contents of the insertrivers.txt changes.
            var paramslist = s.Split(new char[] { ',' }).Select(o => o.Trim()).ToList();

            var sqlparamslist = ins.Parameters.Cast<SqlParameter>().ToList();

            

            for (int i=0; i<input.Count;i++)
            {
                if (i==0)
                {
                    input[0].MapParameters(ins);
                }
                else
                {
                    ins.CommandText += ",(";
                    
                    foreach (string p in paramslist)
                    {
                        ins.CommandText += p + ",";
                    }

                    ins.CommandText = ins.CommandText.Substring(0,ins.CommandText.Length - 1);
                    ins.CommandText += ")";

                    sqlparamslist.ForEach(p =>
                    {
                        ins.Parameters.Add(new SqlParameter(p.ParameterName + i.ToString(), p.SqlDbType));
                    });

                    input[i].MapParameters(ins, i);
                }
            }

            return ins;
        }

        #endregion StandardInserts


        // they fixed some things, because bulkcopy didnt work the time before it was last 2021.
        // seriously... wtf kind of goddamn scifi horror fiction have you fucking idiots made the country ???
        // or is it just screwed up here for no goddamn reason right now?
        // alexandria, va has a bunch of idiots in glasses on the damn beach.
        // they try so hard to seeem so fucking stupid.

        #region BulkLoad

        public static SqlBulkCopy GetBulkCopier(DataTable dt, SqlConnection scon)
        {
            SqlBulkCopy sb = new SqlBulkCopy(scon);

            sb.DestinationTableName = "Rivers";

            foreach (DataColumn dc in dt.Columns)
            {
                sb.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
            }

            return sb;
        }

        public static DataTable FillTable(List<RiversRecord> records, DataTable dt)
        {
            records.ForEach(rec =>
            {
                var dr = dt.NewRow();
                dr["ObjectId"] = rec.OBJECTID;
                dr["Name"] = rec.Name;
                dr["StateAbbreviation"] = rec.State;
                dr["Region"] = rec.Region;
                dr["Miles"] = rec.Miles;
                dr["ShapeLength"] = rec.Shape__Len;
                dr["Shape"] = rec.ShapeInfo==null ? null: SqlGeography.STGeomFromText(new SqlChars( new SqlString(  rec.ShapeInfo.GetWKT())),4326);
              
                var bounding = rec.ShapeInfo!= null ? rec.ShapeInfo.GetExtent() : null;

                if (bounding != null)
                {
                    dr["MinLatitude"] = bounding.X1;
                    dr["MinLongitude"] = bounding.Y1;
                    dr["MaxLatitude"] = bounding.X2;
                    dr["MaxLongitude"] = bounding.Y2;
               }

                dt.Rows.Add(dr);

            });

            return dt;
        }

        public static DataTable GetTable(SqlConnection scon)
        {
            DataTable dt = new DataTable();

            SqlCommand scom = new SqlCommand("select top 1 * from dbo.Rivers", scon);

            var r = scom.ExecuteReader();


            for (int x = 0; x < r.FieldCount; x++)
            {
                string name = r.GetName(x);

                if (name != "Id")
                {
                    Type t = r.GetFieldType(x);
                    dt.Columns.Add(name, name == "Shape" ? typeof(SqlGeography) : t);
                }
            }

            r.Close();

            return dt;
        }

        #endregion BulkLoad

        public static List<RiversRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false, bool resume = false, bool dogc = false)
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

            List<int> objectids = new List<int>();

            if (resume)
            {
                SqlCommand getids = new SqlCommand("select objectid from dbo.Rivers order by objectid", scon);
                var dr = getids.ExecuteReader();

                while (dr.Read())
                {
                    objectids.Add((int)dr[0]);
                }

                dr.Close();
            }

           
            while (dread.Read())
            {

                RiversRecord pr = new RiversRecord();
                pr.Read(dread);

                if (resume)
                {
                    
                    if (!pr.OBJECTID.HasValue)
                    {
                        // this should never ever fire.
                        Console.WriteLine("Missing ObjectId");
                    }

                    // pretty sure they all have objectids, i dont know why the datasource is nullable
                    if (objectids.Contains((pr.OBJECTID.HasValue ? pr.OBJECTID.Value : -1)))
                    {
                        if (eventmode)
                        {
                            SkipRecord(pr);
                            objectids.Remove(pr.OBJECTID.Value);
                        }
                        continue;
                    }

                    if (objectids.Count==0)
                    {
                        resume = false;
                    }
                }

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

                if (dogc)
                {
                    GC.AddMemoryPressure(200000000);
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                }
            }

            dread.Close();

            return results;
        }

        #endregion SuperCeded
        
    }
}
