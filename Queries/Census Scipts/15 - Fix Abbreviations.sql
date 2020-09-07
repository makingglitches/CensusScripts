use Geography
Go

select ce.City, ce.State, usc.City, usc.State from dbo.Census2018 ce
left join dbo.USCities usc
on usc.City = ce.City and usc.State = ce.State
where usc.City is null 

select * from dbo.USCities where City like '% Joe' 


select * from dbo.USCities where Zip='46785'

-- 198 rows that have the abbreviation

--select * from dbo.Census2018  ce
--left join dbo.USCities usc
--on usc.City = ce.City
--where ce.City like 'St.%' and usc.City is null

--select * from dbo.Census2018  ce
--left join dbo.USCities usc
--on usc.City = ce.City
--where ce.City like 'St.%' and usc.City is not null

--select * from dbo.USCities where City like 'St.%'
--select * from dbo.Census2018 where City like 'St.%'


update dbo.Census2018 set City = REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

update dbo.USCities set City =  REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

go