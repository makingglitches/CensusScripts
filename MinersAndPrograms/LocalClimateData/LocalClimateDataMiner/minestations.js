// this is code that gets injected into the browser so that ir runs as a local script
// these are the .ajax parameters that i had to drag out of the
// the included js files on the site itself
// the injected script needs to update the limit and offset fields of the data

// https://www.ncdc.noaa.gov/cdo-web/datatools/lcd

// every time the browser navigates this needs inserted back into the page.

// this calls the stations retrieval web service
// lcd.json contains the results of calling this function numerous times.
function CallStations(_limit, _offset) {
	c = {
		async: true,
		data: { limit: _limit, offset: _offset, datasetid: 'LCD', locationid: 'FIPS:US', sortfield: 'name' },
		headers: { token: '0x2a' },
		traditional: true,
		url: '/cdo-web/api/v2/stations'
	};

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

// this sets the download options on the view cart page
function DownloadOptionsPage(startdate, enddate) {
	$('#LCD_CUSTOM_CSV').click();
	$('#selectedStartDate').val(startdate);
	$('#selectedEndDate').val(enddate);
	$('.cartButton').click();
}

// this submits the selected stations and sets the email address
function DownloadSubmission(email) {
	$('#email').val(email);
	$('#emailConfirmation').val(email);
	document.getElementById('rememberEmail').checked = true;
	$('#buttonSubmit').click();
}



// this navigates to the cart contents
function GoToCart() {
	$('#cartPreviewButton').click();
}

// this function pushes data to the lcd's session stack, selecting the statioid for download
function addCartData(stationid) {
	var ajaxparams = {
		contentType: 'application/json',
		data: JSON.stringify({
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
			console.log('Error');
			console.log(e);
		}
	};

	console.log('Calling Cart Item POST');
	$.ajax(ajaxparams);
}
