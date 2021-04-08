using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace CensusFiles.Loaders
{
   public class SpeciesSeasonLoader:GenericLoader
    {

        private Dictionary<string, string> speciesArchiveGuid = new Dictionary<string, string>();

        public SpeciesSeasonLoader(bool resume, bool emptytable, string inputDirectory)
            : base(new LoaderOptions()
            {
                Resume = resume,
                EmptyTable = emptytable,
                FileDirectory = inputDirectory,
                ConsoleLogging = true,
                TableName = "SpeciesSeason",
                DerivedSqlClause= "concat(DownloadGuid,'-',Season) as ResumeKey",
                DerivedSqlKey=true,
                DerivedResumeKey = true,
                SqlResumeId="ResumeKey"
            }
            )
        {

            this.ProcessRecord += SpeciesSeasonLoader_ProcessRecord;
            this.GetNewRecord = () => { return new SpeciesSeasonRecord(); };
            
            this.DerivedKeyGenerator = (dbf, loader) => 
            {

                PopulateArchiveKeyHash();
                
                string dguid = speciesArchiveGuid[loader.DBFFileName];

                return dguid + "-" + dbf["SeasonCode"].ToString();
            };
        }

        private void SpeciesSeasonLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            PopulateArchiveKeyHash();

            var s = (SpeciesSeasonRecord)r;

            s.DownloadGuid = speciesArchiveGuid[Path.GetFileName(g.ZipFileName)];
        }

        private void PopulateArchiveKeyHash()
        {
            if (speciesArchiveGuid.Count == 0)
            {
                SqlConnection scon = Options.MakeConnection();
                scon.Open();
                SqlCommand scom = new SqlCommand("select DownloadGuid, ArchiveName from dbo.Species", scon);

                var dr = scom.ExecuteReader();

                while (dr.Read())
                {
                    speciesArchiveGuid.Add(dr["ArchiveName"].ToString(), dr["DownloadGuid"].ToString());
                }

                dr.Close();
                scon.Close();
            }


        }

    }
}
