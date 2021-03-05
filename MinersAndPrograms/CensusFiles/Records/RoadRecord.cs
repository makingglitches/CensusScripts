﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DbfDataReader;
using ShapeUtilities;
using System.IO;

namespace CensusFiles
{
    public class RoadRecord : RoadBase,IRecordLoader
    {
        public string FipsId { get; set; }

        public PolyLineShape ShapeInfo { get; set; }

        public static event Action<RoadRecord> OnParse;
        public static event Action<long> OnFileLength;

        public static List<RoadRecord> ParseDBFFile(string filename, SqlConnection scon, bool loadShapeFile = false, bool resetMissingFips = false, bool eventmode = false)
        {
            string[] pieces = filename.Split('_');
            string stcountycode = pieces[2];

            string statecode = stcountycode.Substring(0, 2);
            string countycode = stcountycode.Substring(2, 3);

            ShapeFile shpfile = null;

            if (loadShapeFile)
            {

                string shapefilename = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".shp";
                shpfile = new ShapeFile(shapefilename);
                shpfile.Load();
            }

            DbfDataReaderOptions ops = new DbfDataReaderOptions()
            {
                SkipDeletedRecords = true
            };

            DbfDataReader.DbfDataReader dread = new DbfDataReader.DbfDataReader(filename, ops);

            List<RoadRecord> results = new List<RoadRecord>();

            string fipsid =
            FipsKeyRecord.GetCountyByState(statecode, scon).Where(o => o.State == statecode && o.County == countycode).Select(o => o.FipsId).FirstOrDefault();

            int shpfileindex = 0;

            if (eventmode)
            {
                OnFileLength(dread.DbfTable.Header.RecordCount);
            }

            while (dread.Read())
            {
                RoadRecord r = new RoadRecord();
                r.Read(dread);

                r.FipsId = fipsid;

                if (shpfile != null)
                {
                    r.ShapeInfo = (PolyLineShape)shpfile.Records[shpfileindex].Record;
                    shpfileindex++;
                }

                if (eventmode)
                {
                    OnParse(r);
                }
                else
                {
                    results.Add(r);
                }

                r = null;
                GC.AddMemoryPressure(200000000);
                GC.Collect();
                GC.WaitForFullGCComplete();
            }

            dread.Close();

            return results;

        }

        public static SqlCommand GetInsert(SqlConnection scon)
        {
            // insert into dbo.Roads(Shape)
            // Values(geography::STGeomFromText('LINESTRING(-122.5360 47.656, -122.343 47.656)', 4326))

            string cmd = File.ReadAllText("Queries\\InsertRoad.txt");

            SqlCommand insertcmd = new SqlCommand(cmd, scon);

            insertcmd.Parameters.Add("@LinearId", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FullName", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@RouteType", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@FeatureCode", System.Data.SqlDbType.NVarChar);
            insertcmd.Parameters.Add("@Shape", System.Data.SqlDbType.NVarChar);

            insertcmd.Parameters.Add("@MinLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MinLat", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLon", SqlDbType.Float);
            insertcmd.Parameters.Add("@MaxLat", SqlDbType.Float);


            return insertcmd;
        }

        public void MapParameters(SqlCommand insertcmd)
        {
            object fipser = string.IsNullOrEmpty(FipsId) ? DBNull.Value as object : this.FipsId as object;

            object geomstring =
           ShapeInfo == null ? DBNull.Value as object :
           //"geography::STGeomFromText('" + 
           ShapeInfo.GetWKT();
            //+ "',4122)";

           

            insertcmd.Parameters["@LinearId"].Value = this.LINEARID;
            insertcmd.Parameters["@FullName"].Value = this.FULLNAME;
            insertcmd.Parameters["@RouteType"].Value = this.RTTYP;
            insertcmd.Parameters["@FeatureCode"].Value = this.MTFCC;
            insertcmd.Parameters["@Shape"].Value = geomstring;

            var bounding = geomstring != DBNull.Value ? ShapeInfo.GetExtent() : null;

            insertcmd.Parameters["@MinLon"].Value = bounding != null ? (object)bounding.X1 : DBNull.Value;
            insertcmd.Parameters["@MinLat"].Value = bounding != null ? (object)bounding.Y1 : DBNull.Value;
            insertcmd.Parameters["@MaxLon"].Value = bounding != null ? (object)bounding.X2 : DBNull.Value;
            insertcmd.Parameters["@MaxLat"].Value = bounding != null ? (object)bounding.Y2 : DBNull.Value;


        }

        public void PutRecord(DataTable tgt)
        {
            throw new NotImplementedException();
        }
    }
}