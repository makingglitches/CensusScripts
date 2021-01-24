use Geography
Go

-- find exceptions and missing us city and census matchups
-- need to find a geoid database to determine what is inside what.
-- cdps are sometimes villages and managed properties and jails that dont have 
-- zip codes.
select ce.City, ce.State, usc.City, usc.State from dbo.Census2018 ce
left join dbo.USCities usc
on usc.City = ce.City and usc.State = ce.State
where usc.City is null 
--and ce.City like 'five%'

--select * from dbo.USCities where City like 'five%'
--select * from dbo.Census2018 where City like 'five%'

select City, State from dbo.Census2018 where City like '%County%'


-- update 'st' to 'saint' to standardize names which are not standard in either data source in all cases.

update dbo.Census2018 set City = REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

update dbo.USCities set City =  REPLACE(City COLLATE Latin1_General_CS_AS,'St.' COLLATE Latin1_General_CS_AS,'Saint')
where City like 'St.%'

go


--select * from openrowset('C:\Users\John\Documents\QrCode\Input\Tiger2019PlacesFuckPedophileAssholes\tl_2019_01_place.dbf')