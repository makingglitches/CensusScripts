const console = require('console');
const { app, BrowserWindow,ipcMain } = require('electron');
const { onrejectionhandled } = require('globalthis/implementation');
const { stdin, stdout } = require('process');
const readline = require('readline');
var fs = require('fs');



for (var i in ipcMain.rawListeners)
{
	console.log(ipcMain.rawListeners[i]);
}

console.log("App Started");

console.log(process.cwd())
var miningjs = fs.readFileSync('C:\\Users\\John\\Documents\\CensusProject\\QrCode\\MinersAndPrograms\\LocalClimateData\\LocalClimateDataMiner\\minestations.js');

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

function injectJs(js) {
	var prom = win.webContents
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

function LoadMain()
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

		mainwindow = createWindow();
		mainwindowLoadPromise = mainwindow.loadFile('index.html');

		mainwindowLoadPromise
			.then((value) => {
				console.log('Main Window Loaded');
			})
			.catch((value) => {
				console.log('Error Loading');
			});
	
}

app.whenReady().then( (value)=>
{
	LoadMain();
}
)

app.on('window-all-closed', () => {
	if (process.platform !== 'darwin') {
		app.quit();
	}
});

app.on('activate', () => {
	if (BrowserWindow.getAllWindows().length === 0) {
		LoadMain();
	}
});

app.on('browser-window-created', () => {
	console.log('Browser Window Created');
});
