"use strict";

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

var fs = require("fs");

var _require = require("typescript"),
    resolveTripleslashReference = _require.resolveTripleslashReference;

var _require2 = require("querystring"),
    stringify = _require2.stringify;

var dbstructure = [];
var filename = ".\\Input\\us-zip-code-latitude-and-longitude.json";
var classname = "UsCityRecord";
var tsfilename = "USCityRecords.ts";

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
  console.log(datype);

  for (var i in jsonobject) {
    if (datype != _typeof(jsonobject[i])) {
      datype = typeof string === "undefined" ? "undefined" : _typeof(string);
      break;
    }
  }

  return tabs + "public " + name + ":Array<" + datype + ">=[];\n";
}

function processClass(jsonobject, classname, tablevel) {
  var loadMethod = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : false;
  console.log("in process class:" + classname);
  var runcode = "";
  var tabs = makeTabs(tablevel);
  runcode += tabs + "export class " + classname + "\n";
  runcode += tabs + "{\n";
  runcode += processConstructor(jsonobject, tablevel + 1);
  var fieldcode = processFields(jsonobject, tablevel + 1);
  runcode += fieldcode.runcode;

  if (loadMethod) {
    runcode += "static loadFromFile(filename:string)\n" + "{\n" + tabs + "var json = fs.readFileSync(filename);\n" + tabs + "var collection = JSON.parse(json.toString());\n\n" + tabs + "let ret:Array<USCityRecord> = [];\n" + "\n" + tabs + "for (var i in json)\n" + "{\n" + tabs + "ret.push(new USCityRecord(json[i]));\n" + "\n}\n" + tabs + "return ret;\n";
    "}\n}\n";
  }

  runcode += "\n" + tabs + "}\n\n";
  console.log("leaving processclass:" + classname);
  console.log("runcode length:" + runcode.length + " classcode length:" + fieldcode.classcode.length);
  return {
    runcode: runcode,
    classcode: fieldcode.classcode
  };
} //https://public.opendatasoft.com/explore/dataset/us-zip-code-latitude-and-longitude/api/\


var json = fs.readFileSync("us-zip-code-latitude-and-longitude.json");
var collection = JSON.parse(json);
var code = processClass(collection[0], "USCityRecord", 0, true);
code.classcode = "import fs from 'fs'\n\n" + code.classcode;
fs.writeFileSync(tsfilename, code.classcode + "\n" + code.runcode);