use Geography
go

declare @shdarch as table(fname nvarchar(300))

insert into @shdarch(fname)
select ArchiveName from dbo.Species
group by ArchiveName having count(*) >1

select * from @shdarch

select 
s.*, concat('wget --server-response -q -O - ',s.DownloadLink,' 1>val.txt') as checkfilename,
concat('wget --content-disposition ',s.DownloadLink) as download
from dbo.species s
where exists(select null from @shdarch t where t.fname=s.ArchiveName)
order by s.ArchiveName


