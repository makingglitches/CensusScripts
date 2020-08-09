import QrCode from "qrcode-reader";
import fs from "fs";
import jimp from 'jimp'
import {QrReadResult} from './QrReadResult'

export class QRUnpacker {
	constructor(public files: string[], public outpath: string) {}

	public Unpack(): void {

		var qr = new QrCode();

		qr.callback = function (error: any, readresult: QrReadResult) {
			if (error) {
				console.log(error);
				return;
			}
			console.log(readresult.result);
		};

	

		var buffer = fs.readFileSync("test.png");

		jimp.read(buffer, function (err: any, image: any) {
			if (err) {
				console.error(err);
			}
		
			qr.decode(image.bitmap);
		});
	}
}
