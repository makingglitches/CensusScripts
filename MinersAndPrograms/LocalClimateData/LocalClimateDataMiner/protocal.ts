
export enum MessageType{RecordCount, DownloadOptions, StartJob, JobFinished,JobProgress}


export class PRecordCount
{
    public ptype:MessageType = MessageType.RecordCount;
    public stationcount:number=0;
    

    public constructor(_stationcount:number)
    {
        this.stationcount = _stationcount;
    }

    // i have to progress from this point with all my reminders.
    // the information flows so freely through my head when i'm in this traumatized state
    // derived from knowing i have scum i associate with my rapsists all around me
    // now i have to find a way to move on from here and simply live in defiance
    // of their desire to have everyone forget they were here or around their fucked up asses
    // the only way is to continue life past this point from this place, with everything i created
    // whether they drop me back in pa or not this laptop cannot be stolen again.

    // so ways they 


}

export class PDownloadOptions
{
    public ptype:MessageType = MessageType.DownloadOptions;
    public StartDate:Date;
    public EndDate:Date;
    public StationIds:Array<number>;
    public StartId:number;
    public EndId:number;
    public totalDays:number;

    public PDownloadOptions(startdate:Date,enddate:Date, startid:number,endid:number)
    {
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId=endid;

        this.totalDays = this.EndDate.valueOf() - this.EndDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    }

    // so why exactly do they cut shit off like assholes again ?
    // it would be so fucking simple to just let the world move forward
    // and even when they didnt have an excuse right around the REAL 'great recession' they started fucking 
    // everything up.

    // maybe teaching utter hatred for anyone in black and gold is the right way to go since everyone else here is 
    // a fucking weak ass coward that will give after they realize pushing hard enough 
    // leads to them being hurt.

    public StationDays():number
    {
        return this.StationIds == null ? this.totalDays * (this.EndId-this.StartId+1)
        : this.totalDays* this.StationIds.length;
    }

}

// the newer his dark materials season 2 sends the right message
// even if zimmerman indicates they kept taking assshots of a young teen
// because it was adapted by perverts.
// they always find a way of cheapening everything.