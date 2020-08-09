"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.QRPackager = void 0;
var qrcode_1 = __importDefault(require("qrcode"));
var path_1 = __importDefault(require("path"));
var QRPackager = /** @class */ (function () {
    function QRPackager(files, outputpath) {
        this.files = files;
        this.outputpath = outputpath;
        this.fileWritten = false;
        this.onLog = function (s) { return console.log(s); };
    }
    QRPackager.prototype.Generate = function () {
        this.fileWritten = false;
        var outputname = path_1.default.join(this.outputpath, "test.png");
        console.log("writing: " + outputname);
        this.fileCreated = this.fileCreated.bind(this);
        qrcode_1.default.toFile(outputname, "testtext", this.fileCreated);
        console.log("This should be synchronous,but its not");
        //while(!this.a)
        //{
        //    console.log("no write yet")
        // }
    };
    QRPackager.prototype.fileCreated = function (err) {
        console.log("file written");
        this.fileWritten = true;
    };
    return QRPackager;
}());
exports.QRPackager = QRPackager;
