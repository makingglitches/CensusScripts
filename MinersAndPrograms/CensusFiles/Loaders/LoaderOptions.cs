using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace CensusFiles.Loaders
{
   public class LoaderOptions
    {

        public List<string> FieldsToExclude = new List<string>()
        { 
            "Id"
        };

        public Dictionary<string, Type> FieldsToManuallyType = new Dictionary<string, Type>()
        {
            {"Shape", typeof(SqlGeometry) }
        };

        /// <summary>
        /// The fieldname of the id that is indicative of a unique record. eg. objectid
        /// </summary>
        public string SqlResumeId { get; set; }

        /// <summary>
        /// The fieldname of the id that is indicative of a unique record in the dbase file.
        /// </summary>
        public string DbaseResumeId { get; set; }

        public LoaderOptions()
        {
            RecordLimit = 500;
            EmptyTable = false;
            Resume = true;
            TempDirectoryName = "output";
            LoadShapeFile = true;

            SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder()
            {
                DataSource="localhost",
                IntegratedSecurity=true,
                InitialCatalog="Geography"
            };

            ConnectionString = scb.ConnectionString;
        }


        /// <summary>
        /// The directory in which the original zip can be found
        /// </summary>
        public string FileDirectory { get; set; }

        /// <summary>
        /// The subdirectory to create to place the files to be processed
        /// </summary>
        public string TempDirectoryName { get; set; }

        /// <summary>
        /// Load the accompanying shapefile ?
        /// </summary>
        public bool LoadShapeFile { get; set; }

        /// <summary>
        ///  How many records to load at one time using bulkload.
        /// </summary>
        public int RecordLimit { get; set; }

        /// <summary>
        /// Run a truncate on the table prior to beginning loading.
        /// </summary>
        public bool EmptyTable { get; set; }
        
        /// <summary>
        /// Resume from last record loaded ?
        /// </summary>
        public bool Resume { get; set; }

        /// <summary>
        /// SQL Table Name, assumed .dbo schema
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Sql Server Connection String
        /// </summary>
        public string ConnectionString { get; set; }

    }
}
