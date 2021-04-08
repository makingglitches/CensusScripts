// open species data page in opera or chrome
//https://gapanalysis.usgs.gov/apps/species-data-download/
// select 'all' on the display tab.. this is a lucky thing.
// download wget for windows 
// https://eternallybored.org/misc/wget/
// run the below code.

// creates an empty array
a=[]

$('tbody').children('tr').children('.range-data').parent().each(
    function(i,tr)
    {
        var o = {
            DlLink:  $(tr).children('.range-data').children('a').attr('href'),
            Name:$(tr).children('.common-name')[0].innerText,
            Taxonomy:$(tr).children('.taxa')[0].innerText,
            ScientificName:$(tr).children('.scientific-name')[0].innerText
        }

        a.push(o);
    }
);

console.log(JSON.stringify(a))

// copy the outputted string in a file speciesdownload.json
// run GenerateWgetInput.js
// create a subdirectory 'zips'
// cd zips
// run ../wget.exe --content-disposition -i ../downloadlist.txt