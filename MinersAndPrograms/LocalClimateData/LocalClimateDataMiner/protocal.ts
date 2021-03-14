
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
    public readonly MaxDays:number = 10 * 365 -7;

    constructor(startdate:Date,enddate:Date, startid:number,endid:number)
    {
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId=endid;

        this.totalDays = this.EndDate.valueOf() - this.EndDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    }

    public StationDays():number
    {
        return this.StationIds == null ? this.totalDays * (this.EndId-this.StartId+1)
        : this.totalDays* this.StationIds.length;
    }

    public JobDates():Array<Job>
    {
        var jobs:Array<Job>=[];

        var numberJobs = Math.ceil(  this.StationDays()/this.MaxDays);
        
        var daysperjob = Math.floor( this.totalDays / numberJobs);
        var odddays = (this.totalDays/numberJobs - daysperjob)*numberJobs;

        console.log((daysperjob*numberJobs+odddays));

        return jobs;
    }

}
