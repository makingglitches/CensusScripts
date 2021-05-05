use Geography
go
truncate table dbo.GNISClasses
GO

insert into dbo.GNISClasses (Class,Description)
select t1.Class,t1.Description from 
openrowset(
BULK 'C:\Users\John\Documents\CensusProject\QrCode\Input\GNIS_Class_Types.csv',
FIRSTROW=2,
fieldquote='"',
FORMAT='CSV',
FORMATFILE='C:\Users\John\Documents\CensusProject\QrCode\GNISClasses.fmt'
--,ERRORFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\Errors.log'
)t1

GO
