import fs from 'fs'


export class fields
{
	constructor(jsonobject:any)
	{
		this["city"]=jsonobject["city"];
		this["zip"]=jsonobject["zip"];
		this["dst"]=jsonobject["dst"];
		this["geopoint"]=jsonobject["geopoint"];
		this["longitude"]=jsonobject["longitude"];
		this["state"]=jsonobject["state"];
		this["latitude"]=jsonobject["latitude"];
		this["timezone"]=jsonobject["timezone"];
	}
	//Sample Value:Cove
	public city:string="";

	//Sample Value:71937
	public zip:string="";

	//Sample Value:1
	public dst:number=0;

	public geopoint:Array<number>=[];

	//Sample Value:-94.39398
	public longitude:number=0;

	//Sample Value:AR
	public state:string="";

	//Sample Value:34.398483
	public latitude:number=0;

	//Sample Value:-6
	public timezone:number=0;



}


export class geometry
{
	constructor(jsonobject:any)
	{
		this["type"]=jsonobject["type"];
		this["coordinates"]=jsonobject["coordinates"];
	}
	//Sample Value:Point
	public type:string="";

	public coordinates:Array<number>=[];



}


export class USCityRecord
{
	constructor(jsonobject:any)
	{
		this["datasetid"]=jsonobject["datasetid"];
		this["recordid"]=jsonobject["recordid"];
		this["_fields"]=new fields(jsonobject["fields"]);
		this["_geometry"]=new geometry(jsonobject["geometry"]);
		this["record_timestamp"]=jsonobject["record_timestamp"];
	}
	//Sample Value:us-zip-code-latitude-and-longitude
	public datasetid:string="";

	//Sample Value:6a0a9c66f8e0292a54c9f023c93732f1b41d8943
	public recordid:string="";

	public _fields:fields;


	public _geometry:geometry;


	//Sample Value:2018-02-09T09:33:38.603-07:00
	public record_timestamp:string="";


static loadFromFile(filename:string)
{
var json = fs.readFileSync(filename);
var collection = JSON.parse(json.toString());

let ret:Array<USCityRecord> = [];

for (var i in json)
{
ret.push(new USCityRecord(json[i]));

}
return ret;

}

