
use Geography
go

truncate table dbo.Species
go

declare @json nvarchar(max)

SELECT @json= BulkColumn
 FROM OPENROWSET (BULK 'C:\Users\John\Documents\CensusProject\QrCode\Input\speciesdatawithfilename.json', SINGLE_CLOB) as j;

 insert into dbo.Species(DownloadLink,CommonName,Kingdom,ScientificName,ArchiveName)
select j.DownloadLink, j.CommonName, j.Kingdom, j.ScientificName, j.ArchiveName from openjson(@json)  with( DownloadLink nvarchar(500) '$.DlLink', CommonName nvarchar(100) '$.Name', Kingdom nvarchar(50) '$.Taxonomy', 
			ScientificName nvarchar(100) '$.ScientificName', ArchiveName nvarchar(200) '$.RangeArchiveName') j
	
	
	--"DlLink": "https://www.sciencebase.gov/catalog/file/get/59f5ec6fe4b063d5d307e70b",
	--"Name": "Zebra-tailed Lizard",
	--"Taxonomy": "Reptile",
	--"ScientificName": "Callisaurus draconoides",
	--"RangeArchiveName": "Zebra_tailedLiz.zip",
	--"Downloaded": true