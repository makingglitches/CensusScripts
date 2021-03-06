using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;

namespace CensusFiles.Loaders
{
    public class RiversLoader : GenericLoader
    {

        public RiversLoader(bool resume, bool emptytable, string inputDirectory)
            : base(new LoaderOptions()
            {
                Resume = resume,
                EmptyTable = emptytable,
                FileDirectory = inputDirectory,
                ConsoleLogging = true,
                TableName = "Rivers",
                DbaseResumeId = "OBJECTID",
                SqlResumeId = "ObjectId"
            }
            )
        {
            this.GetNewRecord = () => (IRecordLoader)new RiversRecord();
            this.ProcessRecord += RiversLoader_ProcessRecord;
           
        }

        private void RiversLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseRecord shape)
        {
            RiversRecord rec = (RiversRecord)r;
            rec.ShapeInfo = (PolyLineShape) shape;
        }
    }
}
