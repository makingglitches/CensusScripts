
var fs = require('fs')
const { execSync } = require('child_process');
const { exec } = require('node:child_process');


// if i decide to work on this this time this is just going to be 
// an asynchronous download queue manager class
// really simple concept, this fire and forget idea is rather stupid.
// something has to ensure that things get done properly.
// the only reason i'm using js at all is its the best place to work 
// with json, which obviously the script meant to be fired on the client side
// via the inpsect function or via an automated browser window, generates.

class DownloadMgr
{

     test()
    {
        this.SpeciesData 
    }

    constructor(speciesdata)
    {
        
        this.SpeciesData  = speciesdata;
        this.Queue = [];

    }

}

exports.DownloadMgr=DownloadMgr;