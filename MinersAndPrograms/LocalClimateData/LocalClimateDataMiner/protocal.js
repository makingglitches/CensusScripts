"use strict";
exports.__esModule = true;
exports.PDownloadOptions = exports.Job = exports.PRecordCount = exports.MessageType = void 0;
var MessageType;
(function (MessageType) {
    MessageType[MessageType["RecordCount"] = 0] = "RecordCount";
    MessageType[MessageType["DownloadOptions"] = 1] = "DownloadOptions";
    MessageType[MessageType["StartJob"] = 2] = "StartJob";
    MessageType[MessageType["JobFinished"] = 3] = "JobFinished";
    MessageType[MessageType["JobProgress"] = 4] = "JobProgress";
})(MessageType = exports.MessageType || (exports.MessageType = {}));
var PRecordCount = /** @class */ (function () {
    function PRecordCount(_stationcount) {
        this.ptype = MessageType.RecordCount;
        this.stationcount = 0;
        this.stationcount = _stationcount;
    }
    return PRecordCount;
}());
exports.PRecordCount = PRecordCount;
var Job = /** @class */ (function () {
    function Job(startdate, enddate, startid, endid) {
        this.MsPerDay = 24 * 60 * 60 * 1000;
        this.Started = false;
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId = endid;
    }
    Job.prototype.StationYears = function () {
        return ((this.EndId - this.StartId + 1) *
            (this.EndDate.valueOf() -
                this.StartDate.valueOf() + this.MsPerDay)) / 1000 / 3600 / 24 / 365;
    };
    return Job;
}());
exports.Job = Job;
var PDownloadOptions = /** @class */ (function () {
    function PDownloadOptions(startdate, enddate, startid, endid) {
        this.ptype = MessageType.DownloadOptions;
        // just about 10 years worth of days.
        // the max the service allows.
        this.MaxDays = 10 * 365 - 7;
        this.MsPerDay = 24 * 60 * 60 * 1000;
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId = endid;
        this.totalDays = this.EndDate.valueOf() - this.StartDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    }
    PDownloadOptions.prototype.StationDays = function () {
        return this.StationIds == null ? this.totalDays * (this.EndId - this.StartId + 1)
            : this.totalDays * this.StationIds.length;
    };
    // god i hate this fucking old drunken bastard. and the rest of them so goddamned much.
    PDownloadOptions.prototype.JobDates = function () {
        var jobs = [];
        var numberJobs = Math.ceil(this.StationDays() / this.MaxDays);
        var daysperjob = Math.floor(this.totalDays / numberJobs);
        var odddays = (this.totalDays / numberJobs - daysperjob) * numberJobs;
        // i dont know how to not be repetitive while this is going on
        // people have been doing the same freakish shit forever and a day now.
        // with of course the alterations not involving much but the newest/oldest batch of annoyance
        // and they are all guilty of theft and several other felonies because they take valid complaints
        // and try to make them into instructions for what to do, which of course they will not be doing
        // thanks much.
        // good this adds up.
        //  console.log((daysperjob*numberJobs+odddays));
        var currdate = this.StartDate;
        // the job dates are inclusive.
        for (var x = 0; x < numberJobs; x++) {
            // the date range is inclusive once passed to lcd site
            // they will grab data from 0000 hours of the start date  to 2359 hours of the end date. 
            var nextdate = new Date(currdate.valueOf() + (daysperjob - 1) * this.MsPerDay);
            var j = new Job(currdate, nextdate, this.StartId, this.EndId);
            jobs.push(j);
            currdate = new Date(nextdate.valueOf() + this.MsPerDay);
        }
        return jobs;
    };
    return PDownloadOptions;
}());
exports.PDownloadOptions = PDownloadOptions;
