USE [Geography]
GO

/****** Object:  Table [dbo].[FipsKeys]    Script Date: 9/10/2020 10:06:06 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FipsKeys]') AND type in (N'U'))
DROP TABLE [dbo].[FipsKeys]
GO

/****** Object:  Table [dbo].[FipsKeys]    Script Date: 9/10/2020 10:06:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FipsKeys](
	[SummLevel] [char](3) NOT NULL,
	[State] [char](2) NOT NULL,
	[County] [char](3) NOT NULL,
	[CountyDivision] [char](5) NOT NULL,
	[PlaceCode] [char](5) NOT NULL,
	[ConsolidatedCity] [char](10) NOT NULL,
	[AreaName] [varchar](100) NOT NULL,
	[FipsId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_FipsKeys] PRIMARY KEY CLUSTERED 
(
	[FipsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [FipsKey] ON [dbo].[FipsKeys]
(
	[SummLevel] ASC,
	[State] ASC,
	[County] ASC,
	[CountyDivision] ASC,
	[PlaceCode] ASC,
	[ConsolidatedCity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [PlaceCodeIndex] ON [dbo].[FipsKeys]
(
	[PlaceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


-- MS SQL REMAINS INANE IN SOME WAYS. 

INSERT INTO DBO.FipsKeys(SummLevel,State,County,CountyDivision,PlaceCode,ConsolidatedCity,AreaName)
SELECT T1.*
      FROM  OPENROWSET(BULK  'C:\Users\John\Documents\QrCode\Input\GeoCodesFips\2017Geocodes.csv',  
	  format='csv',
     FORMATFILE='C:\Users\John\Documents\QrCode\Input\GeoCodesFips\bcp.fmt'    
       ) t1

go

update dbo.StateLookup
set Fips = f.State
from  FipsKeys as f 
where 
f.County='000' and
f.CountyDivision='00000'and 
f.PlaceCode='00000' and 
f.ConsolidatedCity='00000'
and f.State <>'00'
and F.AreaName = StateLookup.StateName

go



