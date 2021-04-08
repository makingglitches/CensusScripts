using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Loaders
{
    public class StateLoader:GenericLoader
    {

        // they think i'm playing along with their dumb asses apparently because i was polluted as a form of abuse with some of their
        // bullshit mongrel language overlay out of malicious intent
        // not surprisingly I go back to be the truthfully imperfect but natural human being regardless which i'm trying to work through this
        // to disentangle the stupid they added from the my minds functioning.
        // possible for me, i didnt get told shit till i was in my real 40s.

        // you know there is a name for a population of people that cannot dissent
        // there is also a favorite little african american colloquial term for those that take the table scraps for better position.
        public StateLoader(bool resume, bool emptytable, string inputDirectory)
           : base(new LoaderOptions()
           {
               Resume = resume,
               EmptyTable = emptytable,
               FileDirectory = inputDirectory,
               ConsoleLogging = true,
               TableName = "States",
               DbaseResumeId = "STATENS",
               SqlResumeId = "GNISKey"
           }
           )
        {
            
            this.ProcessRecord += StateLoader_ProcessRecord;
            this.GetNewRecord = () => (IRecordLoader)new StateRecord();

        }

        string zeropad = "000000000000000000"; 

        private void StateLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            StateRecord state = (StateRecord)r;

            state.FipsId = state.STATEFP + zeropad;
        }
    }
}
