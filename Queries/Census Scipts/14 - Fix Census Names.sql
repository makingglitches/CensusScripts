use Geography
go



-- fuck pretty much all of them find out why

-- so case sensitive searching has a weird syntax in sql server, dont know why they did shit this way.
select  REPLACE(City COLLATE Latin1_General_CS_AS,' city' COLLATE Latin1_General_CS_AS,'')
from dbo.Census2018 where City like '% city'

select REPLACE(t.City COLLATE Latin1_General_CS_AS,' city' COLLATE Latin1_General_CS_AS,'') from (
select City from dbo.Census2018 where City like '%City% city') as t

go

-- good test works.

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' city' COLLATE Latin1_General_CS_AS,''),
CensusClass='city'
where City like '% city'

GO

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' town' COLLATE Latin1_General_CS_AS,''),
CensusClass='town'
where City like '% town'

go

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' CDP' COLLATE Latin1_General_CS_AS,''),
CensusClass='unincorporated'
where City like '% CDP'

go

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' village' COLLATE Latin1_General_CS_AS,''),
CensusClass='village'
where City like '% village'

go

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' borough' COLLATE Latin1_General_CS_AS,''),
CensusClass='borough'
where City like '% borough'

go


update dbo.Census2018
set City = TRIM(City)
Go




