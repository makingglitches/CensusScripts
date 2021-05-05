using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Loaders
{
   public class PlaceLoader:GenericLoader
    {
        public PlaceLoader(bool resume, bool emptytable, string inputDirectory)
            : base(new LoaderOptions()
            {
                Resume = resume,
                EmptyTable = emptytable,
                FileDirectory = inputDirectory,
                ConsoleLogging = true,
                TableName = "Places",
                DbaseResumeId = "PLACENS",
                SqlResumeId = "GNISCode"
            }
            )
        {
            this.GetNewRecord = () => (IRecordLoader)new PlaceRecord();
            this.ProcessRecord += PlaceLoader_ProcessRecord;
        }

        private void PlaceLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            PlaceRecord pr = (PlaceRecord)r;
            pr.FipsId = pr.STATEFP + "00000000" + pr.PLACEFP + "00000";
           
        }
    }
}
