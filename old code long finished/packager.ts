import qrcode, { QRCodeToFileOptions } from 'qrcode';
import path from 'path'
import fs from 'fs'



export type qrdonecallback=(which:QRPackager) => void;

export class QRPackager
{

    //https://www.qrcode.com/en/about/version.html
    public readonly maxQRSize:number=1273;

    public fileWritten:boolean=false;

    constructor(public files:string[], public outputpath:string)
    {

    }  

    public  Generate()
    {
        this.fileWritten=false;

        // actually set up the options, 40 maxes at 177 nodes, but its unclear how much data that is.
        // 1273 max characters
        let options:QRCodeToFileOptions = 
        {   
            version:40,
            type:"png",
            margin:5,
            errorCorrectionLevel:"high",
            width:80
        };

        this.files.forEach(element => {

           let stat=  fs.statSync(element);
           stat.size
            
        });
        

        let outputname = path.join(this.outputpath,"test.png")

        console.log("writing: " +outputname);

       this.fileCreated= this.fileCreated.bind(this);

       

        qrcode.toFile(
            outputname, options, 
            "testtext",
           this.fileCreated
            );
            
        
        console.log("This should be synchronous,but its not")

        //while(!this.a)
        //{
        //    console.log("no write yet")
       // }
 
    }

    public fileCreated(err:Error):void
    {
        console.log("file written")
        this.fileWritten=true
    }

}