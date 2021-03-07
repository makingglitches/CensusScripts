use Geography
go

-- like i said twice now, yeah, they're bursting with happiness from doing the same stupid shit over and over.
-- the only people likely even enjoying this slightly as they rut about their miserable lives are the people who design all of this
-- and even they, one can hope, have a deep black dark hole gnawing away at their center
-- miserable wasteful lazy mother fuckers :)

select script.i, script.TABLE_NAME,script.code from
(select 1 as i,dt.TABLE_NAME, CONCAT('dr["', dc.COLUMN_NAME,'"]= this.') as code  from INFORMATION_SCHEMA.TABLES dt
inner join INFORMATION_SCHEMA.COLUMNS dc
on dc.TABLE_NAME=dt.TABLE_NAME

union

select 0 as i, dt.TABLE_NAME, 'DataRow dr = tgt.NewRow();' as code from INFORMATION_SCHEMA.TABLES dt

union

select 2 as i, dt.TABLE_NAME, 'tgt.Rows.Add(dr);' as code from INFORMATION_SCHEMA.TABLES dt) script

order by script.TABLE_NAME, script.i