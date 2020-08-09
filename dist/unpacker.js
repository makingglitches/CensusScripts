"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.QRUnpacker = void 0;
var qrcode_reader_1 = __importDefault(require("qrcode-reader"));
var fs_1 = __importDefault(require("fs"));
var jimp_1 = __importDefault(require("jimp"));
var QRUnpacker = /** @class */ (function () {
    function QRUnpacker(files, outpath) {
        this.files = files;
        this.outpath = outpath;
    }
    QRUnpacker.prototype.Unpack = function () {
        var qr = new qrcode_reader_1.default();
        qr.callback = function (error, readresult) {
            if (error) {
                console.log(error);
                return;
            }
            console.log(readresult.result);
        };
        var buffer = fs_1.default.readFileSync("test.png");
        jimp_1.default.read(buffer, function (err, image) {
            if (err) {
                console.error(err);
            }
            qr.decode(image.bitmap);
        });
    };
    return QRUnpacker;
}());
exports.QRUnpacker = QRUnpacker;
