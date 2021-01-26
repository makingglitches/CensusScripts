
select * from
openrowset(
BULK 'C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\SNAP_Store_Locations.csv',
FORMAT='CSV',
FIRSTROW=2,
FORMATFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\test.fmt'
--,ERRORFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\Errors.log'
)t1