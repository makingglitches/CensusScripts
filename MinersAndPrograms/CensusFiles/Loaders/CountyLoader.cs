using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Loaders
{
    public class CountyLoader : GenericLoader
    {
        public CountyLoader(bool resume, bool emptytable, string inputDirectory)
            : base(new LoaderOptions()
            {
                Resume = resume,
                EmptyTable = emptytable,
                FileDirectory = inputDirectory,
                ConsoleLogging = true,
                TableName = "Counties",
                DbaseResumeId = "COUNTYNS",
                SqlResumeId = "GNISId",
            }
            )
        {
            this.GetNewRecord = () => (IRecordLoader)new CountyRecord();
            this.ProcessRecord += CountyLoader_ProcessRecord;

            for (int x=0; x < 15; x++)
            {
                blankline += " ";
            }

        }

        private string blankline = string.Empty;

        private void CountyLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            CountyRecord cr = (CountyRecord)r;


            cr.FipsId = cr.STATEFP + cr.COUNTYFP + blankline;
        }
    }
}
