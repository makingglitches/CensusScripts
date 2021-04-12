
var fs = require('fs')
var path = require('path')

var s = path.extname('index.html')


function recursepath(srcpath,filefilter=".h") {

    if (srcpath[srcpath.length-1]!="\\")
    {
        srcpath+="\\";
    }

    var files = [];

	var dir = fs.readdirSync(srcpath, { withFileTypes: true });
    var lfilter = filefilter.toLowerCase();

	for (var i in dir) {

		var name = dir[i].name.toString();
        var isfile = dir[i].isFile();
        var isdir = dir[i].isDirectory();
        var ext = path.extname(name);

		if (isfile && ext==lfilter) 
        {
			files.push(srcpath+name);
		}
		else  if (isdir)
        {
		    files = files.concat(recursepath( srcpath+name));   
		}
	}

	return files;
}

function recurseCreate(apath)
{
    if (!fs.existsSync(apath))
    {
        // go up a level.
        recurseCreate( path.dirname(apath));
        
        console.log("creating directory: "+destination+without);
        fs.mkdirSync(apath);
    }
}


console.log(process.argv[2]);

var headerfiles = recursepath(process.argv[2]);

var destination = process.argv[3];

if (destination[destination.length-1]!="\\")
{
    destination+="\\";
}

var c = 0;

for (var i in headerfiles)
{
    var dname = path.dirname(headerfiles[i]);
    var without = dname.replace(process.argv[2],"");

    if (without[0]=="\\")
    {
        without = without.substr(1);
    }

    if (without[without.length-1]!="\\")
    {
        without+="\\";
    }

    recurseCreate(destination+without);

    console.log("Copying: "+ path.basename(headerfiles[i]));
    c++;
    fs.copyFileSync(headerfiles[i], destination+without+path.basename(headerfiles[i]));
}

console.log("Copied: "+c+ " Finished.")
