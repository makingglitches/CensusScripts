"use strict";
exports.__esModule = true;
exports.PDownloadOptions = exports.PRecordCount = exports.MessageType = void 0;
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
var PDownloadOptions = /** @class */ (function () {
    function PDownloadOptions() {
        this.ptype = MessageType.DownloadOptions;
    }
    PDownloadOptions.prototype.PDownloadOptions = function (startdate, enddate, startid, endid) {
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.StartId = startid;
        this.EndId = endid;
        this.totalDays = this.EndDate.valueOf() - this.EndDate.valueOf();
        this.totalDays = this.totalDays / 1000 / 60 / 60 / 24;
    };
    // so why exactly do they cut shit off like assholes again ?
    // it would be so fucking simple to just let the world move forward
    // and even when they didnt have an excuse right around the REAL 'great recession' they started fucking 
    // everything up.
    // maybe teaching utter hatred for anyone in black and gold is the right way to go since everyone else here is 
    // a fucking weak ass coward that will give after they realize pushing hard enough 
    // leads to them being hurt.
    PDownloadOptions.prototype.StationDays = function () {
        return this.StationIds == null ? this.totalDays * (this.EndId - this.StartId + 1)
            : this.totalDays * this.StationIds.length;
    };
    return PDownloadOptions;
}());
exports.PDownloadOptions = PDownloadOptions;
// the newer his dark materials season 2 sends the right message
// even if zimmerman indicates they kept taking assshots of a young teen
// because it was adapted by perverts.
// they always find a way of cheapening everything.
