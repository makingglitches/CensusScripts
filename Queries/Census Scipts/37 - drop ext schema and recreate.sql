-- for use when clearing old values

use Geography;
go

truncate table  dbo.FileSources

declare @stmts as table (cmd nvarchar(max))


-- drop procedures
insert into @stmts(cmd)
select concat (' drop procedure [ext].[',sp.name,']') from sys.procedures sp
inner join sys.schemas ss
on ss.schema_id=sp.schema_id
where ss.name='ext'


-- drop types
insert into @stmts(cmd)
select concat (' drop type if exists [ext].[',st.name,']') from sys.types st
inner join sys.schemas ss
on ss.schema_id=st.schema_id
where ss.name='ext' and st.is_user_defined=1

-- drop tables
insert into @stmts(cmd)
select concat(' drop table [ext].[',dt.TABLE_NAME,']') from INFORMATION_SCHEMA.TABLES dt where dt.TABLE_SCHEMA='ext'

-- loop and execute
declare @cmd nvarchar(max)

while ( ( select count(*) from @stmts) > 0)
begin
	select top 1 @cmd = cmd from @stmts
	print @cmd
	delete from @stmts where  cmd = @cmd
	exec(@cmd)
end

go

-- see thinking back just filled me with a jolt of happiness
-- that comment was written before i remembered you fucking people were pedophile garbage
-- and i was just paying the strippers lotsa money for some rubby rubby in one of the more 
-- overtly easy going clubs i'd ever been in.
-- i'll be happy like that again once i'm away from these people.
drop schema [ext]

go 

create schema [ext]

go

