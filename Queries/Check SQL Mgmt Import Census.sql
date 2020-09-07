USE [Geography]
GO

SELECT [GeoId]
      ,[PlaceName]
  FROM [dbo].[FilteredCensus2018-newcolumns]
  where try_cast(	TotPop65PlusSexRatio	as float) is null	 or
try_cast(	TotPopRace	as float) is null	 or
try_cast(	TotPopOneRace	as float) is null	 or
try_cast(	TotPopTwoOrMoreRace	as float) is null	 or
try_cast(	TotPopUnder5	as float) is null	 or
try_cast(	TotPop5to9	as float) is null	 or
try_cast(	TotPop10to14	as float) is null	 or
try_cast(	TotPop15to19	as float) is null	 or
try_cast(	TotPop20to24	as float) is null	 or
try_cast(	TotPop25to34	as float) is null	 or
try_cast(	TotPop35to44	as float) is null	 or
try_cast(	TotPop45to54	as float) is null	 or
try_cast(	TotPop55to59	as float) is null	 or
try_cast(	TotPop60to64	as float) is null	 or
try_cast(	TotPop65to74	as float) is null	 or
try_cast(	TotPop75to84	as float) is null	 or
try_cast(	TotPop85Plus	as float) is null	 or
try_cast(	TotPopMedianAge	as float) is null	


GO


