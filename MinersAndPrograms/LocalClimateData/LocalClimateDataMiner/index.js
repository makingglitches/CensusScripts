const console = require('console');
const { app, BrowserWindow, ipcMain } = require('electron');
const { onrejectionhandled } = require('globalthis/implementation');
const { stdin, stdout } = require('process');
const readline = require('readline');
var fs = require('fs');

console.log('App Started');

console.log(process.cwd());

var miningjs = fs.readFileSync(
	'minestations.js'
);

function sendarg(arg)
{
	mainwindow.send('nochochannel',arg)
}

// the folder to download the zips to.
var datafolder = 'C:\\Users\\John\\Documents\\CensusProject\\LocalClimateData';

// the list of stations mined from the site
var stations = [];

var lcdwindow;
var mainwindow;

// load stations json
if (fs.existsSync('lcdprogress.json')) {
	stations = JSON.parse(fs.readFileSync('lcdprogress.json'));
}
else {
	stations = JSON.parse(fs.readFileSync('lcd.json'));
}

function createWindow() {
	return new BrowserWindow({
		width: 800,
		height: 600,
		webPreferences: {
			nodeIntegration: false
		}
	});
}

function createNodeWindow() {
	return new BrowserWindow({
		width: 800,
		height: 600,
		webPreferences: {
			nodeIntegration: true
		}
	});
}


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
	lcdwindow = createWindow();
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
function LoadMain() {


	mainwindow = createNodeWindow();
	mainwindowLoadPromise = mainwindow.loadFile('index.html');

	mainwindowLoadPromise
		.then((value) => {
			console.log('Main Window Loaded');
		
		})
		.catch((value) => {
			console.log('Error Loading');
		});

	
	mainwindow.webContents.session.on('will-download', (event,item,wcontents)=>
	{
		console.log(item.getFilename());

		item.on('done',(event,state)=>
		{
			console.log(this.getFilename());
			console.log(state)
		});

	})

	mainwindow.webContents.on('did-navigate', (event,url,response,status)=>
	{
		console.log('navigated to:'+url)
	});

	ipcMain.on('nochochannel', (event, args) => {
		console.log(args);

		if (args.type=='recordcount')
		{
			event.reply('nochochannel',{type:'recordcount', count:stations.length});
		}
		else if (args.type=='downloadoptions')
		{
			job = args;
			job.started=false;
			job.processed=''
			
		}
	});

	mainwindow.webContents.toggleDevTools();
	
}

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
