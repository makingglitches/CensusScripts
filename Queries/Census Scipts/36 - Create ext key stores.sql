use Geography
go

declare @KeyTable as table(tabname nvarchar(max), colname nvarchar(max), dtype nvarchar(max), charlen  int)


insert into @KeyTable 
select dt.TABLE_NAME, dc.COLUMN_NAME, dc.DATA_TYPE,dc.CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.TABLES dt
	inner join INFORMATION_SCHEMA.COLUMNS dc 
	on dc.TABLE_NAME=dt.TABLE_NAME
	inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
	on kcu.TABLE_NAME = dt.TABLE_NAME and kcu.COLUMN_NAME = dc.COLUMN_NAME
	inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tcu
	on tcu.CONSTRAINT_NAME=kcu.CONSTRAINT_NAME
	where tcu.CONSTRAINT_TYPE='PRIMARY KEY'


declare @t nvarchar(max), @c nvarchar(Max), @d nvarchar(max), @l int, @com nvarchar(max); 

while (  (select count(*) from @KeyTable) > 0)
begin
		-- you know the cruel thing is no matter what with these depressing assholes around
		-- how is anyone to be healthy and happy simultaneously ?
		-- they make everything harder to try to tighten their masters hold on their fucking leashes.

		select  @t=k.tabname, @c=k.colname, @d=k.dtype, @l =k.charlen from @KeyTable k
		
		select @com = concat('CREATE TABLE [ext].[',@t,'Keys',']( [key] ' ,@d,  
		case 
		when @d='int' then ' ' 
		when @d in ( 'char', 'nvarchar','nchar','varchar') then concat('(', trim(CAST(@l as char)),')')
		end,',',
		'FileSourceId nvarchar(300) null,',
		'CONSTRAINT [PK_ext',@t,'] PRIMARY KEY CLUSTERED( [key] ASC)',
		') on [Primary]')

		-- really should still only be one of these
		delete from @KeyTable where tabname=@t and colname=@c

		
		print 'Cmd: '
		print @com

		exec(@com)

end
