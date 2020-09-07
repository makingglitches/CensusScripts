use Geography
Go

-- find exceptions and missing us city and census matchups

select ce.City, ce.State, usc.City, usc.State from dbo.Census2018 ce
left join dbo.USCities usc
on usc.City = ce.City and usc.State = ce.State
where usc.City is null 

select City, State from dbo.Census2018 where City like '%County%'


-- update 'st' to 'saint' to standardize names which are not standard in either data source in all cases.

update dbo.Census2018 set City = REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

update dbo.USCities set City =  REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

go
