const console = require('console');
const { app, BrowserWindow } = require('electron');
const { onrejectionhandled } = require('globalthis/implementation');
const { stdin, stdout } = require('process');
const readline = require('readline');

function createWindow () {
  win = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      nodeIntegration: false
    },
    
  })

  obj = win.loadURL("https://www.ncdc.noaa.gov/cdo-web/datatools/lcd")
  win.webContents.toggleDevTools();
  
  obj.then(
      (value)=> 
        { 
            // when the page finishes loading
            console.log('fulfilled'); 
            console.log(value);

            // apparently the return result mirrors what would be found in the devtools console.
            // so..
            var jobj = win.webContents.executeJavaScript(`var c = {
              async: true,
              data: { limit: 10, offset: 2001, datasetid: 'LCD', locationid: 'FIPS:US', sortfield: 'name' },
              headers: { token: '0x2a' },
              traditional: true,
              url: '/cdo-web/api/v2/stations'
            };
            
            var a = [];
            
            $.ajax(c)
              .done(function(b) {
             //   console.log(b);
                a = b.results;
               // console.log(JSON.stringify(a));
              })
              .always(function() {
                console.log('called');
              });
            `);

            jobj.then( (value=>
            {
              console.log('javascript done')
              a=value;

              win.webContents.executeJavaScript('function geta() {return a;} geta();').then((value)=> { b=value; console.log(b.length);  console.log(JSON.stringify(b));  console.log(value)}).catch((value)=>{console.log('error: '+value)})
            })).catch((value)=>
            {
                console.log('javascript failed.')
            });

        },
       (value)=>
       {
           // if there is an error loading page.
           console.log('rejected'); 
           console.log(value);
        })
  }



app.whenReady().then(createWindow)

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow()
  
  }
})
 
app.on('browser-window-created', () => {
    console.log("Browser Window Created");
});

var quit = false;
var pause=true;



function prompt()
{
  rl.question('Cmd: ', (cmd) =>{

    var lcmd =  (!cmd ?"":toLowerCase(cmd));
    lcmd = lcmd.trim();

    if (lcmd=="quit")
    {

    }
    else
    {
      prompt();
    }
     
     
   });

}

function HandleInput()
{
  console.log("handle input reqached")
  rl = readline.createInterface({
    input:stdin,
    output:stdout
  });

  prompt();

 }


HandleInput();