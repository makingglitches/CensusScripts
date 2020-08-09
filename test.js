const fs = require('fs')
const qrcode = require('qrcode')
const { SSL_OP_SSLEAY_080_CLIENT_DH_BUG } = require('constants')
const { randomBytes } = require('crypto')
const QrCode = require('qrcode-reader')
const jimp = require('jimp')


console.log('1 calling link')

fs.link("test.png","shortindex.js",function()
{
    console.log("2 link done.")
    donestuff.link=true;

    fs.unlink("shortindex.js",function()
    {
        console.log('3 unlink done.')
        donestuff.unlink=true;
    })
})

var donestuff=
{
    link:false,
    unlink:false
}

function waiter()
{
    console.log("4 woke up waiter")
    if (donestuff.link && donestuff.unlink)
    {
            console.log('5 all file process done')
    }
    else
    {
        console.log(" 5 going back to sleep")
        return setTimeout(waiter,200);
    }
}

console.log('6 starting wait timer')

var wait = setTimeout(waiter,200);

console.log('7 process main done')


// loop through differing settings and data lengths until a list of settings
// that both libraries like are the end result

global.maxchars=2597
global.joblist=[]
global.errorlist=[]
global.running=[]


var job=function(_length,_buffer,_errorlevel,_number)
{
    this.Length=_length;
    this.Buffer=_buffer;
    this.ErrorLevel=_errorlevel;
    this.ReadResult = ""
    this.Error=null;
    this.Matches=false;
    this.Filename="";
    this.Status="Not Running";
    this.Number=_number

    this.Write=function()
    {
        // this is what the code calls.
        // the end result will be two sets of arrays of all the jobs once they're done.
        var options = 
        {
            width=this.Length,
            margin = 10,
            errorCorrectionLevel=this.ErrorLevel
        }

        this.Filename="qrout/max"+this.Number+".png"

        qrcode.toFile(this.Filename,options,this.writeCallback.bind(this));
        
    }

    this.writeCallback= function(error)
    {
        if (error)
        {
            this.Error=error;
            this.Status="Write Error"
        }
        else
        {
            // read the code back to test it with qrcode-reader lib

            this.qr = new QrCode();
    
            this.Status="Running"
            this.qr.callback = this.readCallback.bind(this);

            jimp.read(this.Filename,this.imageLoadCallback.bind(this))
        }
        
    }
    
    this.imageLoadCallback = function (error,image)
    {
        if (error)
        {
            this.Error=error;
            this.Status="Jimp Error"   
        }
        else
        {
            this.qr.decode(image)
        }
        
    }

    
    this.readCallback = function (error, readresult)
    {
        if (error) 
        {
            this.Error=error
            global.errorlist.push(this)
            this.Status="Read Error"
        }
        else
        {
            this.Error=null;
            this.Status="Successful"
            this.ReadResult=readresult;
           
            if (this.Buffer.toString()==this.ReadResult)
            {
                global.joblist.push(this);
            }

        }
    }
}


let currchars=0;

while (currchars < global.maxchars)
{
    currchars++;
    let buffer=randomBytes(currchars);

    let currwidth=0;
    global.jobs = 0;

    while (currwidth < 1000)
    {
        currwidth+=100;

        var priorities = ['high','medium','low']
        
        for (i in priorities)
        {
            var j = new job(currwidth,buffer,i,globals.jobs)
            j.
        }
    }

}




console.log(buffer.byteLength)

console.log("filled buffer with "+buffer.length)



 

});


