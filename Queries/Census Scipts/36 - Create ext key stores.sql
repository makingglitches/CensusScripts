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
	where tcu.CONSTRAINT_TYPE='PRIMARY KEY' and dt.TABLE_SCHEMA='dbo'


declare @t nvarchar(max), @c nvarchar(Max), @d nvarchar(max), @l int, @com nvarchar(max), @dt nvarchar(max); 

while (  (select count(*) from @KeyTable) > 0)
begin
		-- you know the cruel thing is no matter what with these depressing assholes around
		-- how is anyone to be healthy and happy simultaneously ?
		-- they make everything harder to try to tighten their masters hold on their fucking leashes.

		select  @t=k.tabname, @c=k.colname, @d=k.dtype, @l =k.charlen from @KeyTable k
		
		select @dt =concat(@d, case 
					when @d='int' then ' ' 
					when @d in ( 'char', 'nvarchar','nchar','varchar') then concat('(', trim(CAST(@l as char)),')')
					end)

		select @com = concat('CREATE TABLE [ext].[',@t,'Keys',']( [keyid] ' ,@dt,',',
		'FileSourceId nvarchar(300) null,',
		'Loaded bit not null default(0),',
		'CONSTRAINT [PK_ext',@t,'] PRIMARY KEY CLUSTERED( [keyid] ASC)',
		') on [Primary]')

		-- really should still only be one of these
		delete from @KeyTable where tabname=@t and colname=@c

		
		print 'Cmd: '
		print @com

		exec(@com)

		select @com = concat('CREATE NONCLUSTERED INDEX [IX_',@t,'Keys_LoadedFilesource] ON [ext].[',@t,'Keys] ( [FileSourceId] ASC,	[Loaded] ASC ) ON [PRIMARY]')

		print 'Index Cmd:'
		print @com
		exec(@com)

		-- this move is questionable but beats waiting 2 hours for an update query to complete on a fucking 8 million record table !
		-- honestly this is a good time to do some testing
		-- i betcha anything individually querying will take forever so thats a nope nope.
		-- the inserts are happening so quickly with bulkin that i betcha this wont add much processing time to each file
		-- since all i want to do is get a back a list of 'loaded' values
		-- might run faster if i were using an identity column honestly.
		-- string comparisons run slower and i dont see any way personally that that could not be the case here
		-- this does give me a chance to prove the best method for handling this scenario however
		-- which is i have a fuck ton of records and i want a list of the ones i need to drag out of the file in question, before ANY processing is done.
		-- dragging back a list of 8 million record ids is not an acceptable option for one because its an anxiety inducing scenario aint it ? lol
		-- thats why we create moving shit that tells the user 'OH UH HEY, SOMETHING IS STILL HAPPENING.. WE THINK !'
		-- would be nice if i could connect to the activity monitor and report semaphores back to the ui !
	
	
		select @com = concat ('DROP PROCEDURE if exists [dbo].[',@t,'_GetLoaded]');

		print 'Cmd Text SP:'
		print @com
		exec (@com)

		select @com = concat ('drop type if exists dbo.',@t,'KeyTableType')
		print @com
		exec(@com)

		select @com = concat('create type dbo.',@t,'KeyTableType as table(keyid ',@dt,')')
		print @com
		exec (@com)


		select @com = concat ('CREATE PROCEDURE [dbo].[',@t,'_GetLoaded] @keysToCheck dbo.',@t,'KeyTableType READONLY ',
		'AS BEGIN SET NOCOUNT ON; ',
		'select t.keyid, case when exists (select null from dbo.',@t,' r where r.',@c,'=t.keyid) then 1 else 0 end as Loaded from @keysToCheck t ',
		'END')

		print @com
		exec (@com)

	
end
