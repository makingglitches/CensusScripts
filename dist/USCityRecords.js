"use strict";
exports.__esModule = true;
exports.USCityRecord = exports.geometry = exports.fields = void 0;
var fs_1 = require("fs");
var fields = /** @class */ (function () {
    function fields(jsonobject) {
        //Sample Value:Cove
        this.city = "";
        //Sample Value:71937
        this.zip = "";
        //Sample Value:1
        this.dst = 0;
        this.geopoint = [];
        //Sample Value:-94.39398
        this.longitude = 0;
        //Sample Value:AR
        this.state = "";
        //Sample Value:34.398483
        this.latitude = 0;
        //Sample Value:-6
        this.timezone = 0;
        this["city"] = jsonobject["city"];
        this["zip"] = jsonobject["zip"];
        this["dst"] = jsonobject["dst"];
        this["geopoint"] = jsonobject["geopoint"];
        this["longitude"] = jsonobject["longitude"];
        this["state"] = jsonobject["state"];
        this["latitude"] = jsonobject["latitude"];
        this["timezone"] = jsonobject["timezone"];
    }
    return fields;
}());
exports.fields = fields;
var geometry = /** @class */ (function () {
    function geometry(jsonobject) {
        //Sample Value:Point
        this.type = "";
        this.coordinates = [];
        this["type"] = jsonobject["type"];
        this["coordinates"] = jsonobject["coordinates"];
    }
    return geometry;
}());
exports.geometry = geometry;
var USCityRecord = /** @class */ (function () {
    function USCityRecord(jsonobject) {
        //Sample Value:us-zip-code-latitude-and-longitude
        this.datasetid = "";
        //Sample Value:6a0a9c66f8e0292a54c9f023c93732f1b41d8943
        this.recordid = "";
        //Sample Value:2018-02-09T09:33:38.603-07:00
        this.record_timestamp = "";
        this["datasetid"] = jsonobject["datasetid"];
        this["recordid"] = jsonobject["recordid"];
        this["_fields"] = new fields(jsonobject["fields"]);
        this["_geometry"] = new geometry(jsonobject["geometry"]);
        this["record_timestamp"] = jsonobject["record_timestamp"];
    }
    USCityRecord.loadFromFile = function (filename) {
        var json = fs_1["default"].readFileSync("us-zip-code-latitude-and-longitude.json");
        var collection = JSON.parse(json.toString());
        var ret = [];
        for (var i in json) {
            ret.push(new USCityRecord(json[i]));
        }
        return ret;
    };
    return USCityRecord;
}());
exports.USCityRecord = USCityRecord;
