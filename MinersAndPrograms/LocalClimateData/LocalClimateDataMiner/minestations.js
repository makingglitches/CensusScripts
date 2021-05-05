// this is code that gets injected into the browser so that ir runs as a local script
// these are the .ajax parameters that i had to drag out of the
// the included js files on the site itself
// the injected script needs to update the limit and offset fields of the data

// https://www.ncdc.noaa.gov/cdo-web/datatools/lcd

// every time the browser navigates this needs inserted back into the page.

// this calls the stations retrieval web service
// lcd.json contains the results of calling this function numerous times.

// they limit station data to 10 years or less
// 7:20
// would appear precicipitation land based stations are limited and do not use the wban prefix but the coop
// the rem strings are covered in the lcd documentation pdf so may be where simply the presence of precipitation
// such as freezing rain and the like is something to be discovered.

// is it me or did personal interest in nearly everything seem to drop off around the time these garbage fucks took over ?
// the quality of data available seems to have declined slightly
// probably because they dont see a need to advertise they're fucking with the weather to make everyone
// mute blind sheep that are totally convinced of their own freaksih superiority
// and keep them living in their little chomo fantasy world.
// see this is an example of them interfering
// the gathering of all the necessary data reference to make sense of 
// the lcd data to get the most concise interpretation of weather interpretations
// the remark column data being included.
// apparently not only do they not want to be intelligent they're not interested in anyone 
// else making themselves any smarter.
// and they can scoff all they want at my efforts the final product if i wasn't goddamned exhausted from
// marching around most of the day in the cold would be reusuable.
// this marks something like the 5th time we've reached this point.
// because they ruined the country.
// they really do have some weirdass fantasies.
// 'oh their network is extremely fast'... but... they... drive around.. in circles... and give up...
// half their lives.. by choice... to be.. fucking... slaves ZIMMERMAN!
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

// 3rd
// this sets the download options on the view cart page
function DownloadOptionsPage(startdate, enddate) {
	$('#LCD_CUSTOM_CSV').click();
	$('#selectedStartDate').val(startdate);
	$('#selectedEndDate').val(enddate);
	$('.cartButton').click();
}


// 4th 
// this submits the selected stations and sets the email address
function DownloadSubmission(email) {
	$('#email').val(email);
	$('#emailConfirmation').val(email);
	document.getElementById('rememberEmail').checked = true;
	$('#buttonSubmit').click();
}

// 2nd 
// this navigates to the cart contents
function GoToCart() {
	$('#cartPreviewButton').click();
}


// 1st
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


//5th
//clicks the 'check order status' button that appears after placing an order.
function CheckStatusButton()
{
	$('.statusButton')[0].click();
}


//6th if still processing
function CheckStatusAgain()
{
	$('[data-bind*="resetPaginationAndGetOrders"]')[0].click();
}


//7th check until this is 'Complete'
function GetJobStatus()
{
	return $('td[data-bind*="item.status"]')[0].innerText;
}

//8th
//click download button
function ClickToDownload()
{
	$('a[data-bind*="$root.click(\'download\'"]')[0].click();
}

// i should really just make something that scans the document and checks all this stuff.
