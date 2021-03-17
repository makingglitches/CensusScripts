import { TouchBarScrubber } from "electron";

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
    
    public DownloadName:string;

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

    public readonly MaxDays:number = Math.floor(10 * 365.25) -7; // padded by a week for safety, the service is quirky
    private readonly MsPerDay:number = 24*60*60*1000;
    private readonly MaxStations10year:number =  this.MaxDays;

// an they chose one african american with no accent to play the bad guy.... lol
    public stationDays:number;
    public daysPerDivision:number;

    // i am sooo distracted by the dumbest film i';ve vere seen twice lol
    // seriously..
    constructor(startdate:Date,enddate:Date, startid:number,endid:number)
    {
        this.StartDate = startdate;
        this.EndDate = enddate;

        this.StartId = startid;
        this.EndId=endid;

        this.totalDays = this.EndDate.valueOf() - this.StartDate.valueOf();
 
        this.totalDays = this.totalDays / this.MsPerDay;

        // the number of days requested x total number of stations
        this.stationDays = this.totalDays* (this.EndId-this.StartId);

        this, 


        // yeah the key would be to ask if he'd like to show someone his work.
        // under normal circumstances
        // or basically to make a point of speaking with him more.
        // until that became the case.
        
        var idsegments:Array<int> = [];
        

        // maybe just need a break since i've been walking about 8 miles a day or more 
        // and not working in places free of distraction. sigh.
        // not to mention i dont like the chomo assholes wandering around.





        // the max days requested at once rounded to lowest nearest integer
        var wholedaysperjob = Math.floor(this.stationDays/this.MaxDays );
    
        // DAMN I'M WORKING SLOW. WHY ???
        // NORMALL I FLY ALONG AT A PRETTY FAST RATE !
        // distracted by my environment
        // thinking about alot of awful things...

        if ( wholedaysperjob < 1)
        {

        }
        


    }

    // instead of being shitty about a good time, why not just have the good time and not expect fucked up things ?
    // the trade is having someone who gives a shit for awhile.
    // i walk around hating everyone in sight for a good goddamn reason because i'm convinced they're all
    // psycho garbage thanks to experience/zim/tori/others 
    // would be nice to be able to just relax happily and contentedly like when i was young
    // and didnt know what these people were or rather not just that but what  they were pretty much
    // allowed to do that they should not be doing.
    // only a completely evil, selfish, cynical, self serving bastard would allow them to 
    // perpetuate the ruination that created most of them just to get off.
    // there is no excuse for any of this or anyone.

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
