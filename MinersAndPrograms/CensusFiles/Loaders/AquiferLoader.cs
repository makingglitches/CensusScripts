using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Loaders
{
   public class AquiferLoader:GenericLoader
    {
        public AquiferLoader(bool resume, bool emptytable, string inputDirectory)
         : base(new LoaderOptions()
         {
             Resume = resume,
             EmptyTable = emptytable,
             FileDirectory = inputDirectory,
             ConsoleLogging = true,
             TableName = "Aquifer",
             DbaseResumeId = "OBJECTID",
             SqlResumeId = "ObjectId"
         }
         )
        {
            this.GetNewRecord = () => (IRecordLoader)new AquiferRecord();

        }
    }
}
