using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Loaders
{
    public class RoadsLoader:GenericLoader
    {

        

        public RoadsLoader(bool resume, bool emptytable, string inputDirectory)
            : base(new LoaderOptions()
            {
                Resume = resume,
                EmptyTable = emptytable,
                FileDirectory = inputDirectory,
                ConsoleLogging = true,
                TableName = "Roads",
                DbaseResumeId = "LINEARID",
                SqlResumeId = "StCtyLinId",
                DerivedResumeKey=true
            }
            )
        {

            // sometimes when some dumbfuck pervert sits down and asks 'are you ok' I remember these people all have a screw loose
            // and i think ever so strongly about driving a knife into their eyesocket while screaming the question of what the fuck is wrong with thmem
            // i'd say 'you' but they might misunderstand that in their pervert fucking mongrel overlay of fucking english they apparnetly stopped
            // learning in its proper form in fucking 2nd grade !
            // dumb mother fuckers.
            // why are they not in prison being raped again ?
            // thanks much for reminding me of that detail you fucking asshole.
            // guess he likes the passive aggressive rip on him.
            this.ReportFinalStats += RoadsLoader_ReportFinalStats;
            this.GetNewRecord = () => (IRecordLoader)new RoadRecord();
            this.ProcessRecord += RoadsLoader_ProcessRecord;

            this.ResetKeyFields = () =>
            {
                statecode = string.Empty;
                countycode = string.Empty;
            };

            // yes this was a good idea TORI
            this.DerivedKeyGenerator = (db, lo) =>
            {
               
                if (string.IsNullOrEmpty(statecode))
                {
                    string[] pieces = lo.DBFFileName.Split(new char[] { '_' });
                    string stcountycode = pieces[2];

                    statecode = stcountycode.Substring(0, 2);
                    countycode = stcountycode.Substring(2, 3);

                }

                return statecode+"-"+countycode+"-"+db["LINEARID"];
            };
        }

        private void RoadsLoader_ReportFinalStats(int wrote, int skipped, double totalseconds, double recordswrotepersecond)
        {
            statecode = string.Empty;
            countycode = string.Empty;
        }

        private string lastfile = string.Empty;
        private string statecode = string.Empty;
        private string countycode = string.Empty;
        private string fipspad = string.Empty;

        private void RoadsLoader_ProcessRecord(GenericLoader g, int index, IRecordLoader r, ShapeUtilities.BaseShapeRecord shape)
        {
            RoadRecord rec = (RoadRecord)r;


            if (fipspad.Length == 0)
            {
                for (int x = 0; x < 15; x++) fipspad += "0";
            }

            if (string.IsNullOrEmpty(statecode))
            {
                string[] pieces = g.DBFFileName.Split(new char[] { '_' });
                string stcountycode = pieces[2];

                statecode = stcountycode.Substring(0, 2);
                countycode = stcountycode.Substring(2, 3);

            }

            rec.FipsId = statecode + countycode+fipspad;
            rec.StCtyLinId = statecode + "-" + countycode + "-" + rec.LINEARID;
        }
    }
}
