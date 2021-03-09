using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbfDataReader;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using CensusFiles.Loaders;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace CensusFiles.Helpers
{
    public class KeyProcessor
    {
        private SqlCommand TruncExt;

        private SqlCommand GetMissingIds;
        private SqlCommand InsertFileSource;
        
        private DataTable KeyValues = new DataTable();
        
        public LoaderOptions Options;

        private GenericLoader parent;

        public KeyProcessor(LoaderOptions options, GenericLoader g)
        {
            parent = g;
            Options = options;
            

            TruncExt = new SqlCommand("truncate table [ext].[" + Options.TableName + "Keys]");

            KeyValues.Columns.Add("FileSourceId", typeof(string));

            InsertFileSource = new SqlCommand("InsertFileSource");
            InsertFileSource.Parameters.Add("@filename", SqlDbType.NVarChar);
            InsertFileSource.Parameters.Add("@filesize", SqlDbType.Int);
            InsertFileSource.CommandType = CommandType.StoredProcedure;
        }

        public void LoadKeys(bool Truncate = false)
        {
            string[] zips = Directory.GetFiles(Options.FileDirectory, "*.zip");

            SqlConnection con = Options.MakeConnection();

            SqlBulkCopy bulk = new SqlBulkCopy(con);

            bulk.DestinationTableName = "[ext].[" + Options.TableName + "Keys]";
            bulk.ColumnMappings.Add("key", "key");
            bulk.ColumnMappings.Add("FileSourceId", "FileSourceId");

            con.Open();

            InsertFileSource.Connection = con;

            if (Truncate)
            {
                TruncExt.Connection = con;
                TruncExt.ExecuteNonQuery();
            }

            foreach (string zf in zips)
            {
                Console.WriteLine("Processing: " + Path.GetFileName(zf));
                FileInfo f = new FileInfo(zf);
                InsertFileSource.Parameters["@filename"].Value = zf;
                InsertFileSource.Parameters["@filesize"].Value = f.Length;

                InsertFileSource.ExecuteNonQuery();
                string dbfname = "";
                ZipArchive za = ZipFile.OpenRead(zf);

                foreach (ZipArchiveEntry ze in za.Entries)
                {
                    if (ze.FullName.EndsWith(".dbf"))
                    {
                        dbfname = ze.FullName;
                        ze.ExtractToFile("local.dbf",true);
                    }
                }

                parent.currentDBFName = dbfname;
               
                if (parent.ResetKeyFields!=null)
                {
                    parent.ResetKeyFields();
                }

                DbfDataReader.DbfDataReader db = new DbfDataReader.DbfDataReader("local.dbf", new DbfDataReaderOptions() { SkipDeletedRecords = true });

                while (db.Read())
                {
                    DataRow dr = KeyValues.NewRow();

                    object key = Options.DerivedResumeKey ? parent.DerivedKeyGenerator(db, parent) : db[Options.DbaseResumeId];

                    if (KeyValues.Columns.Count ==1)
                    {
                        KeyValues.Columns.Add("key", key.GetType());
                    }

                    dr["key"] = key;
                    dr["FileSourceId"] = dbfname;

                    KeyValues.Rows.Add(dr);
                }

                db.Close();

                Console.WriteLine("Loaded " + KeyValues.Rows.Count.ToString() + " keys");
                bulk.WriteToServer(KeyValues);
                Console.WriteLine("Wrote to server");

                KeyValues.Rows.Clear();
            }




            con.Close();
        }


    }
}
