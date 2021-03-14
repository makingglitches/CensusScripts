
export enum MessageType{RecordCount, DownloadOptions, StartJob, JobFinished,JobProgress}

export class PRecordCount
{
    public ptype:MessageType = MessageType.RecordCount;
    public stationcount:number=0;
    

    public constructor(_stationcount:number)
    {
        this.stationcount = _stationcount;
    }

}

export class Job
{
    public Started:boolean;
    public StartDate:Date;
    public EndDate:Date;
    public StartId:number;
    public EndId:number;
    private MsPerDay:number = 24*60*60*1000;
    

    public StationYears():number
    {
        return ((this.EndId- this.StartId+1)* 
        ( this.EndDate.valueOf() -  
          this.StartDate.valueOf() + this.MsPerDay)) / 1000/3600/24/365;
    }

    constructor(startdate:Date, enddate:Date, startid:number, endid:number)
    {
        this.Started=false;
        this.StartDate=startdate;
        this.EndDate=enddate;
        this.StartId=startid;
        this.EndId=endid;
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

    // just about 10 years worth of days.
    // the max the service allows.
    public readonly MaxDays:number = (10 * 365) -7;
    private readonly MsPerDay:number = 24*60*60*1000;

    constructor(startdate:Date,enddate:Date, startid:number,endid:number)
    {
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId=endid;

        this.totalDays = this.EndDate.valueOf() - this.StartDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    }

    public StationDays():number
    {
        return this.StationIds == null ? this.totalDays * (this.EndId-this.StartId+1)
        : this.totalDays* this.StationIds.length;
    }

    // god i hate this fucking old drunken bastard. and the rest of them so goddamned much.

    public JobDates():Array<Job>
    {
        var jobs:Array<Job>=[];

        var numberJobs = Math.ceil(  this.StationDays()/this.MaxDays);
        
        var daysperjob = Math.floor( this.totalDays / numberJobs);

        // this is how many days forward to set the last job.
        var odddays = (this.totalDays/numberJobs - daysperjob)*numberJobs;

        // i dont know how to not be repetitive while this is going on
        // people have been doing the same freakish shit forever and a day now.
        // with of course the alterations not involving much but the newest/oldest batch of annoyance
        // and they are all guilty of theft and several other felonies because they take valid complaints
        // and try to make them into instructions for what to do, which of course they will not be doing
        // thanks much.
        // good this adds up.
       //  console.log((daysperjob*numberJobs+odddays));

        var currdate:Date = this.StartDate;

        // the job dates are inclusive.

        for (var x:number=0; x < numberJobs; x++)
        {
            // the date range is inclusive once passed to lcd site
            // they will grab data from 0000 hours of the start date  to 2359 hours of the end date. 
            var nextdate:Date = new Date( currdate.valueOf() + ( daysperjob-1)*this.MsPerDay );
            
            var j:Job = new Job(currdate,nextdate,this.StartId,this.EndId);

            jobs.push(j);

            currdate = new Date(nextdate.valueOf()+this.MsPerDay);
        }
        
        return jobs;
    }

}
