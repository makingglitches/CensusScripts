
use Geography
GO

declare @json as nvarchar(max)

SELECT @json= BulkColumn
 FROM OPENROWSET (BULK 'C:\Users\Rescue\Documents\QrCode\Input\us-zip-code-latitude-and-longitude.json', SINGLE_CLOB) as j;

 insert into dbo.store(text) values (@json)

 GO
