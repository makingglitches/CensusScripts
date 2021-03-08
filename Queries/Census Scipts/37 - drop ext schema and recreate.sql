-- for use when clearing old values

use Geography;


truncate table  dbo.FileSources

declare @stmts as table (cmd nvarchar(max))

insert into @stmts(cmd)
select concat(' drop table [ext].[',dt.TABLE_NAME,']') from INFORMATION_SCHEMA.TABLES dt where dt.TABLE_SCHEMA='ext'

declare @cmd nvarchar(max)

while ( ( select count(*) from @stmts) > 0)
begin
	select top 1 @cmd = cmd from @stmts
	delete from @stmts where  cmd = @cmd
	exec(@cmd)
end

go

drop schema [ext]

go 

create schema [ext]

go

