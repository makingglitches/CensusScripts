// this program generates a list of urls from the 
// species data webpage, and then saves then to a json file
// when run again, it generates another file 
// which tracks which have been finished downloading
// using the wget tool 
// it follows the content disposition headers returned from the
// server to ensure the correct filename is set when each zip is downloaded


var fs = require('fs');
const { execSync } = require('child_process');
const { time } = require('console');

// loads the speciesdownload file first
var json = "" 
var speciesdata = []

if (fs.existsSync('speciesdatawithfilename.json')) {
	 json = fs.readFileSync('speciesdatawithfilename.json');
}
else
{
	json=fs.readFileSync('speciesdownload.json');
}

speciesdata = JSON.parse(json);

var downloadList = '';

var timems = 1000;

for (var i in speciesdata) {
	console.log('Processing: ' + i + ' of ' + speciesdata.length);
	if (!speciesdata[i].RangeArchiveName) {
		var cmd = '.\\wget --server-response -q -O - ' + speciesdata[i].DlLink + ' 1>dumby.txt 2>value.txt';

		var dumby = execSync(cmd, { encoding: 'utf-8', maxBuffer: 1024 });

		var output = fs.readFileSync('value.txt').toString();

		var namestart = output.indexOf('filename=') + 10;
		var clpos = output.indexOf('Content-Length');
		var len = clpos - namestart - 3;

		var sub = output.substr(namestart, len);
		sub = sub.substr(0, sub.indexOf('"'));

		console.log(sub);

		speciesdata[i].RangeArchiveName = sub;
		fs.writeFileSync('speciesdatawithfilename.json', JSON.stringify(speciesdata));
	}

	if (!speciesdata[i].Downloaded) {
		try {
			if (!fs.existsSync('.\\zips')) {
				fs.mkdirSync('.\\zips');
			}

			var getcmd =
				'.\\wget.exe --content-disposition -P .\\zips ' +
				speciesdata[i].DlLink +
                ' 1>dumby.txt 2>value.txt';
                
			execSync(getcmd);
			speciesdata[i].Downloaded = true;
			fs.writeFileSync('speciesdatawithfilename.json', JSON.stringify(speciesdata));
		} catch (e) {
			console.log('Error Downloading Archive');
		}
	}

	downloadList += speciesdata[i].DlLink + '\n';
}

fs.writeFileSync('downloadlist.txt', downloadList);

//console.log(speciesdata);
