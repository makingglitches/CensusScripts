
use Geography
go

truncate table dbo.ClimateStations
GO

declare @json nvarchar(max);


SELECT @json= BulkColumn
 FROM OPENROWSET (BULK 'C:\Users\John\Documents\CensusProject\LocalClimateData\lcd-02-05-2021.json'
, SINGLE_CLOB) as j;


 select * from openjson(@json) 

 insert into dbo.ClimateStations(Elevation,
								 ElevationUnit,
								 Latitude,
								 Longitude,
								 [Name],
								 Stationid, 
								 MinDate)
 select * from openjson(@json)
 with ( Elevation float '$.elevation', 
		ElevationUnit nvarchar(50) '$.elevationUnit',
		Latitude float '$.latitude',
		Longitude float '$.longitude',
		[Name] nvarchar(250) '$.name',
		[Stationid] nvarchar(50) '$.id',
		MinDate date '$.mindate')
  
  --{
	--	"elevation": 15.2,
	--	"mindate": "2009-01-01",
	--	"maxdate": "2021-01-26",
	--	"latitude": 29.976,
	--	"name": "ABBEVILLE CHRIS CRUSTA MEMORIAL AIRPORT, LA US",
	--	"datacoverage": 1,
	--	"id": "WBAN:00184",
	--	"elevationUnit": "METERS",
	--	"longitude": -92.084
	--},
