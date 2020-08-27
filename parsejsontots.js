const fs = require('fs');
const { resolveTripleslashReference } = require('typescript');
const { stringify } = require('querystring');

// now we have to wonder why they decided on windows 10 to be the 'last version' even though
// linux has serious gaming deficiencies.s
// again and they're leaving now... lol which means theyre both of age hmmm
console.log(Math.random())
//ohhh putting some easily recognizable numbers in that eh ?
// predict this to make it weird again lol
console.log(Math.random()*Math.random()*1000000)
//this too
console.log("HEY FUCKING PEDOPHILES OF COLORADO AND ELSEWHERE STOP TRYING TO FUCK JOHN'S HEAD UP OR HE'LL KILL SEVERAL OF THEM")
console.log(Math.random()*100000)
// this too
console.log(Math.random()*10000)
// did this before lol
console.log(Math.random()*100)

var json = fs.readFileSync("us-zip-code-latitude-and-longitude.json")

var collection = JSON.parse(json)

function commentize(value)
{
    if (value==null || value==undefined) { return "";}

    value=value.toString();
    value = value.replace("\n","\n//")
    return value
}

function makeTabs(count)
{
    return repeat("\t",count)
}

function repeat(value, count)
{
    var ret=""

    for (i=0; i < count ; i++)
    {
        ret+=value
    }

    return ret;
}

function processFields(jsonobject,tablevel)
{
   var tabs = makeTabs(tablevel)

   var retvalue = {}

    var code = "";
    var classcode="";

    for (field in jsonobject)
    {
        var datype = typeof jsonobject[field];

       if (datype =="object")
        {
            code+=tabs+"public _"+field+":"+field+";\n\n"

            var notnumber=false;

            for (index in jsonobject[field])
            {
                if (index+1==NaN)
                {
                    notnumber=true;
                    break;
                }
            }

            if (notnumber)
            {
            tcode=processClass(jsonobject[field],field,0)
            classcode+=tcode.classcode;
            classcode+=tcode.runcode;
            }
            else
            {
                tcode=processArray(jsonobject[field])
                code+=tabs+"//array type test"
                code+=tcode.classcode
            }
        }
        else
        {
            code+=tabs+"//Sample Value:" + commentize(jsonobject[field])+"\n"
            code+=tabs+"public "+field+":"+(typeof jsonobject[field])+"="
            
            switch(datype)
            {
                case 'string': 
                    code+="\"\""
                    break;
                case 'number':
                    code+="0"
                    break;
                default:
                    code+="null"
            }       

            code+=";\n"
        }

        code+="\n"
    }

    code+="\n"

    retvalue.runcode=code;
    retvalue.classcode=classcode;



    return retvalue;
}

function processConstructor(jsonobject,tablevel)
{
    var code=""
    var tabs=makeTabs(tablevel)

    code+=tabs+"constructor(jsonobject:any)\n";
    code+=tabs+"{\n";
    
    tabs=makeTabs(tablevel+1)

    for (field in jsonobject)
    {
        var datype = typeof jsonobject[field];
        var isobject = datype=="object"

        code+=tabs+
              "this[\""+(isobject?"_" : "")+
              field+"\"]="+
              (isobject?"new "+field+"(jsonobject[\""+field+"\"])" : "jsonobject[\""+field+"\"]")+";\n"
        
    }

    tabs=makeTabs(tablevel)

    code+=tabs+"}\n"

    return code
}

function processArray(jsonobject,tablevel)
{

}

function processClass(jsonobject,classname,tablevel)
{
    var code = ""
    var tabs = makeTabs(tablevel)

    code+=tabs+"export class "+classname+"\n"
    code+=tabs+"{\n"


    code+=processConstructor(jsonobject,tablevel+1)

    var fieldcode=processFields(jsonobject,tablevel+1)

    code+=fieldcode.runcode;

    code+="\n"+tabs+"\}\n\n";

    return {runcode:code,classcode:fieldcode.classcode}
}

//https://public.opendatasoft.com/explore/dataset/us-zip-code-latitude-and-longitude/api/\


var code =processClass(collection[0],"USCityRecord",0)
console.log(processClass(collection[0],"USCityRecord",0))



fs.writeFileSync("USCityRecord.ts",code.classcode+"\n"+code.runcode)

/*
datasetid - string
recordid
fields
geometry
record_timestamp
*/