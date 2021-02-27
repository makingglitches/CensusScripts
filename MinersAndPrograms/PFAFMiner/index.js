

// the structure of the main search page uses a class 'content' to
// mark eahch <tr> element
// col 1 is the scientific name enclosed within an anchor tag
// col 2 is the common name
// col 3 is edibility rating
// col 4 is the medicinal rating

// the scale for edibility is pretty much anything under 4 might be wood bark.
// that of course does not make it useless, like you can actually
// eat certain types of conifers (the inner bark of them) which 
// well would make sense in a fucking forest filled
// with nothing but goddamn trees and no game in the middle
// of goddamn winter so... this is useful data


// this may not exactly require a bot
// a list of urls could be downloaded with a little cut and paste and a few clicks
// edibility of 4 or greater only yields about 10 pages
// that wont take long

// each page contains a table marked with table-striped class
// this will contain several rows with two columns
// the first column will match a 
// just like the last time i did the initial analysis of content layout
// there will be a plant image link which contains PlantImages in the path 

var recs = [];


        var rows = $('tr.content');

        rows.each(function(irow,row)
        {
            if (irow > 0)
            {
            var cols = $(row).children('td')
            
            var link = $(cols[0]).children('a')
            var lname = link[0].innerText;
            var pageurl = link[0].getAttribute('href')
            var cname = cols[1].innerText;
            var erating = cols[2].innerText;
            var mrating = cols[3].innerText;
            var orating=cols[4].innerText;

            var item=
            {
                LatinName:lname,
                CommonName:cname,
                URL:pageurl,
                EdibleRating:erating,
                MedicinalRating:mrating,
                OtherRating:orating
            }

            recs.push(item)
        }
        })
// copies the json list to clip board.
        copy(JSON.stringify(recs));

