use Geography
Go

truncate table dbo.CensusLookup
go

insert into dbo.CensusLookup(CensusColumn,CensusDescription,TableColumn)
values
('GEO_ID','id','GeoId'),
('NAME','Geographic Area Name','PlaceName'),
('DP05_0032E','Estimate  SEX AND AGE  Total population  65 years and over  Sex ratio (males per 100 females)','TotPop65PlusSexRatio'),
('DP05_0033E','Estimate  RACE  Total population','TotPopRace'),
('DP05_0034E','Estimate  RACE  Total population  One race','TotPopOneRace'),
('DP05_0035E','Estimate  RACE  Total population  Two or more races','TotPopTwoOrMoreRace'),
('DP05_0005E','Estimate  SEX AND AGE  Total population  Under 5 years','TotPopUnder5'),
('DP05_0006E','Estimate  SEX AND AGE  Total population  5 to 9 years','TotPop5to9'),
('DP05_0007E','Estimate  SEX AND AGE  Total population  10 to 14 years','TotPop10to14'),
('DP05_0008E','Estimate  SEX AND AGE  Total population  15 to 19 years','TotPop15to19'),
('DP05_0009E','Estimate  SEX AND AGE  Total population  20 to 24 years','TotPop20to24'),
('DP05_0010E','Estimate  SEX AND AGE  Total population  25 to 34 years','TotPop25to34'),
('DP05_0011E','Estimate  SEX AND AGE  Total population  35 to 44 years','TotPop35to44'),
('DP05_0012E','Estimate  SEX AND AGE  Total population  45 to 54 years','TotPop45to54'),
('DP05_0013E','Estimate  SEX AND AGE  Total population  55 to 59 years','TotPop55to59'),
('DP05_0014E','Estimate  SEX AND AGE  Total population  60 to 64 years','TotPop60to64'),
('DP05_0015E','Estimate  SEX AND AGE  Total population  65 to 74 years','TotPop65to74'),
('DP05_0016E','Estimate  SEX AND AGE  Total population  75 to 84 years','TotPop75to84'),
('DP05_0017E','Estimate  SEX AND AGE  Total population  85 years and over','TotPop85Plus'),
('DP05_0018E','Estimate  SEX AND AGE  Total population  Median age (years)','TotPopMedianAge')
