using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using DbfDataReader;
using ShapeUtilities;
using System.IO;

namespace CensusFiles
{
    public class StateRecord : StateBase, IRecordLoader, IHasShape
    {
        // tori needs to stop trying to insert stupid like 'only load and check one record at a time' bullshit to add anxiety to coding methods
        // i already know how to do bitch
        // fucking weird cunts
        // speaking of course of the resume functionality.
        // soooo i forgot that there were 245,000 records in the roads db
        // sooo i forgot there was a 2100 limit to parameters in sqlcommands so multi inserts wouldnt do
        // soooo i never really had much use for sqlbulkcopy because i was used to just bulk loading csv's for large data ops and/or
        // using builtin backup/restore functionality
        // i'm still experienced enough to know that goddamn checking one fucking id at a time and forging a seperate connection per is a ridiculous
        // waste of time and would be slower than shit !
        public string FipsId { get; set; }

        public BaseShapeRecord Shape { get; set; }

        // besides having said that before this is mostly about burying hateful anxiety causing text in a github database
        // though i want to keep this goddamn code and not have some child molester from colorado or elsewhere or routed through
        // colorado steal it or my identity since they tried their best to ensure i couldnt get back on my feet over and over again
        // when i was being honest which is why they assault me and call the cops first nowadays to limit my travel and escape or progression
        // options just in time for them to roll the date back. fucking assholes.

        public void PutRecord(DataTable tgt)
        {
            DataRow dr = tgt.NewRow();
            dr["Abbreviation"] = this.STUSPS;
            dr["AreaLand"] = this.ALAND;
            dr["AreaWater"] = this.AWATER;
            dr["DivisionCode"] = this.DIVISION;
            dr["FipsKey"] = this.FipsId;
            dr["GNISKey"] = this.STATENS;
            dr["Latitude"] = this.INTPTLAT;
            dr["Longitude"] = this.INTPTLON;
            dr["LSAD"] = this.LSAD;
            dr["Name"] = this.NAME;
            dr["RegionCode"] = this.REGION;
            dr["Shape"] = this.Shape?.GetMSSQLInstance(); 
            
            var bounding = this.Shape?.GetExtent(); 
            
            if (bounding != null) 
            { 
                dr["MinLatitude"] = bounding.X1; 
                dr["MinLongitude"] = bounding.Y1; 
                dr["MaxLatitude"] = bounding.X2; 
                dr["MaxLongitude"] = bounding.Y2; 
            }

            tgt.Rows.Add(dr);
        }
    }
}