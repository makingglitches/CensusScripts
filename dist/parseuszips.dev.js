"use strict";

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

var fs = require("fs");

var _require = require("typescript"),
    resolveTripleslashReference = _require.resolveTripleslashReference;

var _require2 = require("querystring"),
    stringify = _require2.stringify; // second time attempted to guess the age of my offspring and they repeated this
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
    var datype = _typeof(jsonobject[field]);

    console.log("field:" + field + " fieldtype:" + datype);

    if (datype == "object") {
      if (!isArray(jsonobject[field])) {
        runcode += tabs + "public _" + field + ":" + field + ";\n\n";
        tcode = processClass(jsonobject[field], field, 0);
        classcode += tcode.classcode + "\n" + tcode.runcode;
      } else {
        runcode += processArray(jsonobject[field], field, tablevel);
      }
    } else {
      runcode += tabs + "//Sample Value:" + commentize(jsonobject[field]) + "\n";
      runcode += tabs + "public " + field + ":" + _typeof(jsonobject[field]) + "=";

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
  var datype = _typeof(jsonobject);

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
    var datype = _typeof(jsonobject[field]);

    var isobject = datype == "object" && !isArray(jsonobject[field]);
    code += tabs + 'this["' + (isobject ? "_" : "") + field + '"]=' + (isobject ? "new " + field + '(jsonobject["' + field + '"])' : 'jsonobject["' + field + '"]') + ";\n";
  }

  tabs = makeTabs(tablevel);
  code += tabs + "}\n";
  return code;
}

function processArray(jsonobject, name, tablevel) {
  var datype = _typeof(jsonobject[0]);

  var tabs = makeTabs(tablevel);
  console.log(datype); // default to type of string if any value does not match properly
  // this may not work for crappy javascript types that are just shoved full of differing shit
  // but in typesript this should work just fine.

  for (var i in jsonobject) {
    if (datype != _typeof(jsonobject[i])) {
      datype = typeof string === "undefined" ? "undefined" : _typeof(string);
      break;
    }
  }

  return tabs + "public " + name + ":Array<" + datype + ">=[];\n";
}

function processClass(jsonobject, classname, tablevel) {
  console.log("in process class:" + classname);
  var runcode = "";
  var tabs = makeTabs(tablevel);
  runcode += tabs + "export class " + classname + "\n";
  runcode += tabs + "{\n";
  runcode += processConstructor(jsonobject, tablevel + 1);
  var fieldcode = processFields(jsonobject, tablevel + 1);
  runcode += fieldcode.runcode;
  runcode += "\n" + tabs + "}\n\n";
  console.log("leaving processclass:" + classname);
  console.log("runcode length:" + runcode.length + " classcode length:" + fieldcode.classcode.length);
  return {
    runcode: runcode,
    classcode: fieldcode.classcode
  };
} //https://public.opendatasoft.com/explore/dataset/us-zip-code-latitude-and-longitude/api/\


var code = processClass(collection[0], "USCityRecord", 0); //console.log(processClass(collection[0],"USCityRecord",0))

fs.writeFileSync("USCityRecord.ts", code.classcode + "\n" + code.runcode);
/*
datasetid - string
recordid
fields
geometry
record_timestamp
*/