
use Geography
go

update dbo.USCities 
set State = trim(state),
	City = trim(city)
					

go