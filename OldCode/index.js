
const packager = require('./dist/packager')
const unpacker = require('./dist/unpacker')

const fs = require('fs')

if (fs.existsSync("test.png")) {fs.unlinkSync("test.png");}

var pack = new packager.QRPackager(['test.txt'],".")

pack.Generate();

console.log(fs.existsSync("test.png"))

console.log("did await work ?")



function callTime()
{
    if (!this.fileWritten)
    {
        console.log('reached wait. no write.')
        setTimeout(callTime.bind(pack),100)
        // kinda like a fork call
    }
    else
    {
        console.log("moving on")
        var unpack = new unpacker.QRUnpacker(['test.png'],'testout')
        unpack.Unpack()

    }
    
}

if (!pack.fileWritten)
{
   setTimeout(callTime.bind(pack),100)
}





//https://www.amazon.com/gp/product/B07JD1JS5L/ref=ox_sc_act_title_1?smid=A152DJB9ZJI8F3&psc=1
//https://www.amazon.com/gp/product/B002GWF492/ref=ox_sc_act_title_2?smid=ATVPDKIKX0DER&psc=1

