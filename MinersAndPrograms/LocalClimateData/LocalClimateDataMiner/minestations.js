// this is code that gets injected into the browser so that ir runs as a local script
// these are the .ajax parameters that i had to drag out of the 
// the included js files on the site itself
// the injected script needs to update the limit and offset fields of the data 

function setStationParams(_limit,_offset)
{
c = {
	async: true,
	data: { limit: _limit, offset: _offset, datasetid: 'LCD', locationid: 'FIPS:US', sortfield: 'name' },
	headers: { token: '0x2a' },
	traditional: true,
	url: '/cdo-web/api/v2/stations'
};
}

function callStations()
{
// this is where the stations will get stored for recovery
 a = [];

$.ajax(c)
	.done(function(b) {
		console.log(b);
		a = b.results;
		console.log(JSON.stringify(a));
	})
	.always(function() {
		console.log('called');
    });
}


function GoToCart()
{
	$('#cartPreviewButton').click();
}


// this function pushes data to the lcd's session stack, selecting the statioid for download
function addCartData(stationid)
{
	var ajaxparams = {
		contentType: 'application/json',
		data: JSON.stringify( {
			id: stationid,
			type: 'station'
		}),
		type: 'POST',
		url: '/cdo-web/cart/additembyid.json',
		success: function(a) {
			console.log('Success');
			console.log(a);
		},
		error: function(e) {
			console.log("Error")
			console.log(e);
		}
	};

	console.log('Calling Cart Item POST')
	$.ajax(ajaxparams);
}
