use Geography
go

truncate table dbo.StateLookup
go

bulk insert  dbo.StateLookup
from 'C:\Users\John\Documents\QrCode\Input\StateNames.csv'
with ( fieldterminator=',',
	   rowterminator='\n',
	   format='csv'
	   )

go

update dbo.StateLookup set StateName=TRIM(StateName)

go
