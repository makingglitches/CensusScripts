var fs = require('fs')
// call me lazy but sometimes i just add scripts to get around problems in things like visual studio which apparnetly won't take a
//  multiline post build command heh.
if (fs.existsSync(process.argv[2]))
{
    console.log("Found File. Removing: "+process.argv[2]);
    fs.rmSync(process.argv[2]);
}