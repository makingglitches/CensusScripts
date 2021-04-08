// go to https://www.ncdc.noaa.gov/cdo-web/datatools/lcd

const { lchmodSync } = require('fs');

// this will select all items on screen

$('.addToCartButton').not('.disabled').click();

//https://www.ncdc.noaa.gov/cdo-web/api/v2/stations?limit=25&offset=1&datasetid=LCD&locationid=FIPS%3AUS&sortfield=name

// ajax options
// normally limit is 25 but the service accepts 100 at least, offset 1 is start
// apparently will accept 1000, chaneg the offset from 1 to 1001 to 2001 to download all metadata
var c = {
	async: true,
	data: { limit: 1000, offset: 2001, datasetid: 'LCD', locationid: 'FIPS:US', sortfield: 'name' },
	headers: { token: '0x2a' },
	traditional: true,
	url: '/cdo-web/api/v2/stations'
};

var a = [];

$.ajax(c)
	.done(function(b) {
		console.log(b);
		a = b.results;
		console.log(JSON.stringify(a));
	})
	.always(function() {
		console.log('called');
	});

// metadata is now in lcd.json
// so apparently the data being stored is session data.. how, no idea, but this may complicate things slightly
// requiring a bit more tinkering because obviously not clicking 2400+ items.. jesus

// in checking the jquery.min.js file they had extended
// jquery itself to include some custom code, one of these things
// in addition to some weird logic
// does this part here with an ajax call
// the data id is direct from the now contents of lcd.json
// and apparently the cart is being maintained in the
// session state object on the server.
var ajaxparams = {
	contentType: 'application/json',
	data: {
		id: 'WBAN:13962',
		type: 'station'
	},
	type: 'POST',
	url: '/cdo-web/cart/additembyid.json',
	success: function(a) {
		console.log('Success');
		console.log(a);
	},
	error: function(e) {
		console.log(e);
	}
};

// copy and paste from partial files into this variabe on page
var lcdstationids = [];

var lcdindex = 0;
var maxlcdindex=49;
// so we'll copy and past the json data from lcd into an array var
// then

function pushsession() {
	var data = { id: lcdstationids[lcdindex].id, type: 'station' };

	ajaxparams.data = JSON.stringify(data);
	console.log('at ' + lcdindex + ' of ' + lcdstationids.length);
	$.ajax(ajaxparams);
    lcdindex++;
    
	if (lcdindex < lcdstationids.length) {
		console.log('timer cleared');
		timer = setTimeout(pushsession,500);
	}
}

var timer = setTimeout(pushsession, 500);

// after the last index is reached continue with page as normal
