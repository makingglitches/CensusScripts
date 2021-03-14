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
        this.Started = false;
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId = endid;
    }
    return Job;
}());
exports.Job = Job;
var PDownloadOptions = /** @class */ (function () {
    function PDownloadOptions(startdate, enddate, startid, endid) {
        this.ptype = MessageType.DownloadOptions;
        // just about 10 years worth of days.
        // the max the service allows.
        this.MaxDays = 10 * 365 - 7;
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId = endid;
        this.totalDays = this.EndDate.valueOf() - this.EndDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    }
    PDownloadOptions.prototype.StationDays = function () {
        return this.StationIds == null ? this.totalDays * (this.EndId - this.StartId + 1)
            : this.totalDays * this.StationIds.length;
    };
    PDownloadOptions.prototype.JobDates = function () {
        var jobs = [];
        var numberJobs = Math.ceil(this.StationDays() / this.MaxDays);
        var daysperjob = Math.floor(this.totalDays / numberJobs);
        var odddays = (this.totalDays / numberJobs - daysperjob) * numberJobs;
        console.log((daysperjob * numberJobs + odddays));
        return jobs;
    };
    return PDownloadOptions;
}());
exports.PDownloadOptions = PDownloadOptions;
