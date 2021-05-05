USE [Geography]
GO

truncate table dbo.uscities

go

declare @json nvarchar(max)

SELECT @json = [text]
  FROM [dbo].[store]

  select * from openjson(@json)

insert into dbo.USCities(recordid, City, Zip, Dst,Longitude,Latitude,State,RecordStamp,TimeZone)
select  * from openjson(@json)
WITH(  recordid nvarchar(32) '$.recordid', city nvarchar(100) '$.fields.city', zip nvarchar(10) '$.fields.zip',
 dst int '$.fields.dst',  longitude real '$.fields.longitude', latitude real '$.fields.latitude', state nvarchar(2) '$.fields.state',
  record_timestamp nvarchar(max), timezone int '$.fields.timezone')


GO


