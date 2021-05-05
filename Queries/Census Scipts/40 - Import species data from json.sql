
use Geography
go

truncate table dbo.Species
go

declare @json nvarchar(max)

SELECT @json= BulkColumn
 FROM OPENROWSET (BULK 'C:\Users\John\Documents\CensusProject\QrCode\Input\speciesdatawithfilename.json', SINGLE_CLOB) as j;

 insert into dbo.Species(DownloadLink,CommonName,AnimalType,ScientificName,ArchiveName,DownloadGuid,ServerArchiveName)
select j.DownloadLink, j.CommonName, j.AnimalType, j.ScientificName, j.ArchiveName, j.DownloadGuid, j.ServerRangeArchiveName from openjson(@json)  with( DownloadLink nvarchar(500) '$.DlLink', CommonName nvarchar(100) '$.Name', AnimalType nvarchar(50) '$.Taxonomy', 
			ScientificName nvarchar(100) '$.ScientificName', ArchiveName nvarchar(200) '$.RangeArchiveName', ServerRangeArchiveName nvarchar(200) '$.ServerRangeArchiveName',
			DownloadGuid nvarchar(100) '$.DownloadGuid') j
	
-- at this point i dont know if they're just arbitrarily updating things to try to be 'clever'. there were 4 well arranged animal kingdoms when i was a kid...
-- i mean given they're either actually fucking up the interet or they're constraining and filtering content which it would seem people who had nothing to do 
-- but make life, especially their own, pointless being they were lazy unmotivated slobs that like to cause melodrama and make honest people angry and unhappy
-- it would not surprise me.
	
		--"DlLink": "https://www.sciencebase.gov/catalog/file/get/59f5e295e4b063d5d307df6f",
		--"Name": "Townsend's Western Big-eared Bat",
		--"Taxonomy": "Mammal",
		--"ScientificName": "Corynorhinus townsendii townsendii",
		--"RangeArchiveName": "Corynorhinus_townsendii townsendii.zip",
		--"Downloaded": true,
		--"DownloadGuid": "59f5e295e4b063d5d307df6f",
		--"ServerRangeArchiveName": "Townsend_sWeste.zip"