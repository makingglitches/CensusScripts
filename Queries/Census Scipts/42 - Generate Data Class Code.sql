

select dt.*, dc.*, (select null from INFORMATION_SCHEMA. from INFORMATION_SCHEMA.TABLES dt
inner join INFORMATION_SCHEMA.COLUMNS dc
on dc.TABLE_NAME=dt.TABLE_NAME and dc.TABLE_SCHEMA=dt.TABLE_SCHEMA
where dt.TABLE_SCHEMA='dbo'