use Geography
go

-- find the rows that dont match
select * from 
dbo.Census2018 ce 
left join dbo.USCities uc
on  uc.State = ce.State
where uc.City='Cove' and ce.City like 'Cove%'


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

-- now town, might wanna add a unit name field for the results of these ? that can be done if the csv file will still load with an extra column added
-- to table.

select * from dbo.Census2018 c where c.City like '% town'

go

update dbo.Census2018
set City = REPLACE(City COLLATE Latin1_General_CS_AS,' town' COLLATE Latin1_General_CS_AS,'')
where City like '% town'

go

select * from dbo.Census2018