
export enum MessageType{RecordCount, DownloadOptions, StartJob, JobFinished,JobProgress}
// they figured out that as a child they had to either kill someone or 
// imprison me falsely after a very long time to make me forget
// so that is what they have been doing
// which btw leads us to, i saw them cripple someone
// also betrayal by the family which they accomplished through my piece of shit father
// and then later by my sister.


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

// their game is they play like they had nothing to do with something
// but the series of timed and convenient (for them) events that occur 
// along with some very blatant ones say the opposite
// that and these fucking whores pretend sometimes they dont see something
// like some massive chomo get on the bus and punch a 20 something guy who hasnt seen anything out
// and steal his bag.
// larimer county in fort collins is to blame for quite a few things.

// if these files go missing
// if this laptop goes missing
// if john r sohn goes to jail or prison it is an attempt to build up his anxiety regarding losing his work 
// and his memories to facilitate the theft of them by 'unavoidable problems'... like storing a single fucking
// 2x2x3 bag containing diplomas hard disks and a laptop for the umpteenth time which time and time again
// just like the fucked up car they sold, stealing HIS money, they lust after
// so they can send fucking child molesters out from this state with a backstory they can adapt


// and then on top of that these retards seem intent on matching EVERYTHING up to some inane
// numerical schedule.
// i mean it really is ridiculous.
// times, purchases, addresses, everytime they force something to reoccur because god forbid a person have
// free will. like they always did.

export class Job
{
    
    public Started:boolean;
    public StartDate:Date;
    public EndDate:Date;
    public StartId:number;
    public EndId:number;

    constructor(startdate:Date, enddate:Date, startid:number, endid:number)
    {

        this.Started=false;


    }
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
    public readonly MaxDays:number = 10 * 365 -7;

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

    public JobDates():number
    {
        return this.StationDays
    }

}

// the newer his dark materials season 2 sends the right message
// even if zimmerman indicates they kept taking assshots of a young teen
// because it was adapted by perverts.
// they always find a way of cheapening everything.