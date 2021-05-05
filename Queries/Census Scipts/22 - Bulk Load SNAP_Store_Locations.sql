use Geography
GO
 
insert into dbo.SNAP_Store_Locations
	(X,
	Y,
	ObjectId,
	Store_Name,
	Address,
	Address2,
	City,
	State,
	Zip5,
	Zip4,
	County,
	Longitude,
	Latitude)
select 
	try_cast(t1.X as float),
	try_cast(t1.Y as float),
	try_cast(t1.ObjectId as int),
	t1.Store_name,
	t1.Address,
	t1.Address2,
	t1.City,
	t1.State,
	t1.Zip5,
	t1.Zip4,
	t1.County,
	try_cast(t1.Longitude as float),
	try_cast(t1.Latitude as float) 
from
openrowset(
BULK 'C:\Users\John\Documents\CensusProject\SnapData\SNAP_Store_Locations.csv',
FORMAT='CSV',
FIRSTROW=2,
FORMATFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\test.fmt'
--,ERRORFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\Errors.log'
)t1


select cast (1 as nvarchar)