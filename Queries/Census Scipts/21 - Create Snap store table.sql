-- now begins the part where these pieces of shit try to limit charge time blah blah blah
-- because they make yet more associations with their babyrape shit while they wind up to steal our property
-- via brinegar. who is guilty of aiding in this multiple times. as were the mexicans at people ready.
use Geography
go

drop table dbo.SNAP_Store_Locations
go

create table dbo.SNAP_Store_Locations
(
	X float not null,
	Y float not null,
	ObjectId nvarchar(20) primary key not null,
	Store_Name nvarchar(100) not null,
	[Address] nvarchar(100) not null,
	Address2 nvarchar(100) null,
	City Nvarchar(50) not null,
	State Nvarchar(10) not null,
	Zip5 Nvarchar(5) not null,
	Zip4 Nvarchar(4) not null,
	County Nvarchar(50) not null,
	Longitude float not null,
	Latitude float not null
)

go

-- see, said as much.


declare @count int

select @count = count(*) from INFORMATION_SCHEMA.COLUMNS where  TABLE_NAME='SNAP_Store_Locations'

select ORDINAL_POSITION,
case LOWER(t.DATA_TYPE)
when	'char'	then	'SQLCHAR'
when	'varchar'	then	'SQLCHAR'
when	'nchar'	then	'SQLNCHAR'
when	'nvarchar'	then	'SQLNCHAR	'
when	'text'	then	'SQLCHAR'
when	'ntext'	then	'SQLNCHAR'
when	'binary'	then	'SQLBINARY'
when	'varbinary'	then	'SQLBINARY'
when	'image'	then	'SQLBINARY'
when	'datetime'	then	'SQLDATETIME'
when	'smalldatetime'	then	'SQLDATETIM4'
when	'decimal'	then	'SQLDECIMAL'
when	'numeric'	then	'SQLNUMERIC'
when	'float'	then	'SQLFLT8'
when	'real'	then	'SQLFLT4'
when	'int'	then	'SQLINT'
when	'bigint'	then	'SQLBIGINT'
when	'smallint'	then	'SQLSMALLINT'
when	'tinyint'	then	'SQLTINYINT'
when	'money'	then	'SQLMONEY'
when	'smallmoney'	then	'SQLMONEY4'
when	'bit'	then	'SQLBIT'
when	'uniqueidentifier'	then	'SQLUNIQUEID'
when	'sql_variant'	then	'SQLVARIANT'
when	'timestamp'	then	'SQLBINARY'
end,
0, 
coalesce(t.CHARACTER_MAXIMUM_LENGTH, t.NUMERIC_PRECISION),
(case ORDINAL_POSITION when @count then '"\r\n"' else '","' end),
t.ORDINAL_POSITION,
t.COLUMN_NAME,
coalesce(t.COLLATION_NAME,'')
from INFORMATION_SCHEMA.COLUMNS t where  TABLE_NAME='SNAP_Store_Locations'

