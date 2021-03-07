use Geography
go

select dt.TABLE_NAME, CONCAT('dr["', dc.COLUMN_NAME,'"]= this.')  from INFORMATION_SCHEMA.TABLES dt
inner join INFORMATION_SCHEMA.COLUMNS dc
on dc.TABLE_NAME=dt.TABLE_NAME

