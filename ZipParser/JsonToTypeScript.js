const fs = require("fs");
const { resolveTripleslashReference } = require("typescript");
const { stringify } = require("querystring");

// god so sick of working on this over and over. jesus leave our fucking code alone so things can fucking grow 
// fucking sick fucking chomo fucks !

// will contain data about the database structure to create, parent/child elements and fk relationships
// for later database write and table creation methods.
var dbstructure=[];

// second time attempted to guess the age of my offspring and they repeated this
// assholes say 'oh now we believe you' but they're just being cruel piece of shit assholes
// which is their nature
// ohh we could LIE and say yeah... they're great... we could also castrate ourselves but they dont see that happening
// now we have to wonder why they decided on windows 10 to be the 'last version' even though
// linux has serious gaming deficiencies.s
// again and they're leaving now... lol which means theyre both of age hmmm
// console.log(Math.random())
//ohhh putting some easily recognizable numbers in that eh ?
// predict this to make it weird again lol
// console.log(Math.random()*Math.random()*1000000)
// //this too
// console.log("HEY FUCKING PEDOPHILES OF COLORADO AND ELSEWHERE STOP TRYING TO FUCK JOHN'S HEAD UP OR HE'LL KILL SEVERAL OF THEM")
// console.log(Math.random()*100000)
// // this too
// console.log(Math.random()*10000)
// // did this before lol
// console.log(Math.random()*100)

// a note on the 'code' returns, runcode field is meant to contain code that is contained in the present class
// classcode contains the code that will be prepended to dataset class.
// javascript really is an annoying language to trace through by eye.

var filename =
"us-zip-code-latitude-and-longitude.json";

var tsfilename = "USCityRecords.ts";

var json = fs.readFileSync("us-zip-code-latitude-and-longitude.json");

var collection = JSON.parse(json);

function commentize(value) {
	if (value == null || value == undefined) {
		return "";
	}

	value = value.toString();
	value = value.replace("\n", "\n//");
	return value;
}

function makeTabs(count) {
	return repeat("\t", count);
}

function repeat(value, count) {
	var ret = "";

	for (i = 0; i < count; i++) {
		ret += value;
	}

	return ret;
}

function processFields(jsonobject, tablevel) {
    
	console.log("entering processfields");

	var tabs = makeTabs(tablevel);

	var retvalue = {};

	var runcode = "";
	var classcode = "";

	for (field in jsonobject) {
        var datype = typeof jsonobject[field];
        
        console.log("field:" + field + " fieldtype:" + datype);
        
		if (datype == "object") {

			
			if (!isArray(jsonobject[field])) {
				runcode += tabs + "public _" + field + ":" + field + ";\n\n";

				tcode = processClass(jsonobject[field], field, 0);
				classcode += tcode.classcode+"\n"+tcode.runcode;
				
			} else {
				runcode += processArray(jsonobject[field], field, tablevel);
			}
		} else {
			runcode +=
				tabs +
				"//Sample Value:" +
				commentize(jsonobject[field]) +
                "\n";
                
			runcode +=
				tabs +
				"public " +
				field +
				":" +
				typeof jsonobject[field] +
				"=";

			switch (datype) {
				case "string":
					runcode += '""';
					break;
				case "number":
					runcode += "0";
					break;
				default:
					runcode += "null";
			}

			runcode += ";\n";
		}

		runcode += "\n";
	}

	runcode += "\n";

	retvalue.runcode = runcode;
	retvalue.classcode = classcode;

	return retvalue;
}

function isArray(jsonobject) {
	var datype = typeof jsonobject;

	if (datype == "object") {
		var notnumber = false;

		for (index in jsonobject) {
			notnumber = isNaN(index);
			if (notnumber) break;
		}

		return !notnumber;
	} else {
		return false;
	}
}

function processConstructor(jsonobject, tablevel) {
	var code = "";
	var tabs = makeTabs(tablevel);

	code += tabs + "constructor(jsonobject:any)\n";
	code += tabs + "{\n";

	tabs = makeTabs(tablevel + 1);

	for (field in jsonobject) {
		var datype = typeof jsonobject[field];
		var isobject = ( datype == "object" && (!isArray(jsonobject[field])));

		code +=
			tabs +
			'this["' +
			(isobject ? "_" : "") +
			field +
			'"]=' +
			(isobject
				? "new " + field + '(jsonobject["' + field + '"])'
				: 'jsonobject["' + field + '"]') +
			";\n";
	}

	tabs = makeTabs(tablevel);

	code += tabs + "}\n";

	return code;
}

function processArray(jsonobject, name, tablevel) {
	var datype = typeof jsonobject[0];
	var tabs = makeTabs(tablevel);
	console.log(datype);

	// default to type of string if any value does not match properly
	// this may not work for crappy javascript types that are just shoved full of differing shit
	// but in typesript this should work just fine.
	for (var i in jsonobject) {
		if (datype != typeof jsonobject[i]) {
			datype = typeof string;
			break;
		}
	}

	return tabs + "public " + name + ":Array<" + datype + ">=[];\n";
}

function processClass(jsonobject, classname, tablevel,loadMethod=false) {
	console.log("in process class:" + classname);

	var runcode = "";
	var tabs = makeTabs(tablevel);

	runcode += tabs + "export class " + classname + "\n";
	runcode += tabs + "{\n";

	runcode += processConstructor(jsonobject, tablevel + 1);

	var fieldcode = processFields(jsonobject, tablevel + 1);

	runcode += fieldcode.runcode;

	if (loadMethod)
	{
		runcode+= "static loadFromFile(filename:string)\n"+
				  "{\n"+
				  tabs+"var json = fs.readFileSync(filename);\n"+	   
			 	  tabs+"var collection = JSON.parse(json.toString());\n\n"+
				  tabs+"let ret:Array<USCityRecord> = [];\n"+
				  "\n"+
				  tabs+"for (var i in json)\n"+
				   "{\n"+
				   tabs+"ret.push(new USCityRecord(json[i]));\n"+
				   "\n}\n"+
				   tabs+"return ret;\n"
				   "}\n}\n";
		
	}

	runcode += "\n" + tabs + "}\n\n";

	console.log("leaving processclass:" + classname);
	console.log(
		"runcode length:" +
			runcode.length +
			" classcode length:" +
			fieldcode.classcode.length
	);

	return { runcode: runcode, classcode: fieldcode.classcode };
}

//https://public.opendatasoft.com/explore/dataset/us-zip-code-latitude-and-longitude/api/\

var code = processClass(collection[0], "USCityRecord", 0,true);
//console.log(processClass(collection[0],"USCityRecord",0))

code.classcode= "import fs from 'fs'\n\n"+code.classcode;

fs.writeFileSync(tsfilename , code.classcode + "\n" + code.runcode);

/*
datasetid - string
recordid
fields
geometry
record_timestamp
*/
