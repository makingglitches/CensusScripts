const { PDownloadOptions, Job } = require("./protocal");

var pd = new PDownloadOptions(new Date(2009,1,1), new Date(2021,1,1), 1, 1000);

console.log(pd.totalDays);
console.log(pd.StationDays());
console.log(pd.JobDates());

var p = pd.JobDates();

for (var i in p)
{
    console.log( p[i].StationYears());
}
