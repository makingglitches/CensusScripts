
//#region imports
const console = require('console');
const { app, BrowserWindow, ipcMain } = require('electron');
const { onrejectionhandled } = require('globalthis/implementation');
const { stdin, stdout } = require('process');
const readline = require('readline');
var fs = require('fs');
const { PRecordCount, PDownloadOptions } = require('./protocal');
//#endregion imports


console.log('App Started');
console.log(process.cwd());


//#region field initialization

// read in the client side mining js
// this basically is the end result of manual deconstruction
// of the data services which btw are sometimes OBFUSCATED
// this page makes mining pertinent data difficult.
// and of course the api's dont actually allow access to the USEFUL data.
var miningjs = fs.readFileSync(
	'minestations.js'
);

// the folder to download the zips to.
var datafolder = 'C:\\Users\\John\\Documents\\CensusProject\\LocalClimateData';

// the list of stations mined from the site
var stations = [];

// the window that will be displaying the local climate data webpage
var lcdwindow;

// the window that will be displaying the mainwindow to control and query the lcd window
var mainwindow;

// load stations json
if (fs.existsSync('lcdprogress.json')) {
	stations = JSON.parse(fs.readFileSync('lcdprogress.json'));
}
else {
	stations = JSON.parse(fs.readFileSync('lcd.json'));
}

//#endregion field initialization

//#region helper methods
// send something to the main window
function sendarg(arg)
{
	mainwindow.send('nochochannel',arg)
}


// creates  a browser window with or without node integration
function createWindow(nodeint) {
	return new BrowserWindow({
		width: 800,
		height: 600,
		webPreferences: {
			nodeIntegration: nodeint
		}
	});
}


// injects javascript into the lcd window and then returns a promise to implement
function injectJs(js) {
	var prom = lcdwindow.webContents
		.executeJavaScript(js)
		.then((value) => {
			console.log('Javascript Injected');
		})
		.catch((error) => {
			console.log('Error injecting JS');
			console.log('Error: ' + (error ? error : '<empty>'));
		});

	return prom;
}

function LoadLCD()
{
	
	// the site wont load if nodeintegration is on for some reason.
	lcdwindow = createWindow(false);
	lcdLoadPromise = lcdwindow.loadURL('https://www.ncdc.noaa.gov/cdo-web/datatools/lcd');
	lcdwindow.webContents.toggleDevTools();

	lcdLoadPromise.then(
		(value) => {
			console.log('Page Loaded');
			injectJs(miningjs);
		},
		(value) => {
			// if there is an error loading page.
			console.log('Load Error: ' + (value ? value : 'Error'));
		}
	);

	lcdwindow.webContents.on('did-navigate',(event,url,response,status)=>
	{
		console.log("LCD Window Navigated to:"+url)
		injectJs(miningjs);
	});

}

function LoadMain() 
{

	mainwindow = createWindow(true);
	mainwindowLoadPromise = mainwindow.loadFile('index.html');

	mainwindowLoadPromise
		.then((value) => {
			console.log('Main Window Loaded');
		
		})
		.catch((value) => {
			console.log('Error Loading');
		});


		// i can see alot of room for improvement in syntax
		// i should write a module :)
	
	mainwindow.webContents.session.on('will-download', (event,item,wcontents)=>
	{
		console.log("Why is this firing in mainwindow ?");
		console.log(item.getFilename());

		item.on('done',(event,state)=>
		{
			//console.log(this.getFilename());
			console.log(state)
		});

	})

	mainwindow.webContents.on('did-navigate', (event,url,response,status)=>
	{
		console.log('navigated to:'+url)
	});

	
	var jobs=[];

	ipcMain.on('nochochannel', (event, args) => {
		console.log("Message Rcvd:");
		console.log(args);

		


		if (args.type=='recordcount')
		{
		    var min = Number.MAX_SAFE_INTEGER;
			var max = Number.MIN_SAFE_INTEGER;
			
			var pr = new PRecordCount(stations.length,stations);

			event.reply('nochochannel',pr);

		}
		else if (args.type=='downloadoptions')
		{
			job = args;
			job.started=false;
			job.processed=''

			var counts = args.toid - args.fromid +1;

			var startdate =  Date.parse(args.fromdate)
			var enddate = Date.parse(args.todate);

			var days = (enddate - startdate) /1000/3600/24;
			var maxdays = 10*365-7;

			if (counts*days < maxdays)
			{
				jobs.push( job);
			}
			else
			{
				// god i cant think this late in the evening heh.
				
				var jobcount = Math.ceil( (counts*days)/maxdays);

				var daysperjob = days/jobcount;

				var currentstart = startdate;

				var msperyear = 365*24*60*60*1000;

				

			}

		}
	});

	mainwindow.webContents.toggleDevTools();	
}


//#endregion helper methods

//#region application events

app.whenReady().then((value) => {
	LoadMain();
	LoadLCD();
});

app.on('window-all-closed', () => {
	if (process.platform !== 'darwin') {
		app.quit();
	}
});

app.on('activate', () => {
	if (BrowserWindow.getAllWindows().length === 0) {
		LoadMain();
		LoadLCD();
	}
});

app.on('browser-window-created', () => {
	console.log('Browser Window Created');
});

//#endregion application events
