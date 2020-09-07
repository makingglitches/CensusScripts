use Geography
go

truncate table dbo.census2018
go

bulk insert  dbo.Census2018 
from 'C:\Users\John\Documents\QrCode\Input\Census2018.csv'
with ( fieldterminator=',',
	   rowterminator='\n',
	   format='csv'
	   )

go

update dbo.Census2018 set StateName=trim(statename)
GO