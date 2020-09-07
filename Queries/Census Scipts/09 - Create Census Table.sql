USE [Geography]
GO

/****** Object:  Table [dbo].[Census2018]    Script Date: 9/6/2020 6:45:49 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Census2018]') AND type in (N'U'))
DROP TABLE [dbo].[Census2018]
GO

/****** Object:  Table [dbo].[Census2018]    Script Date: 9/6/2020 6:45:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Census2018](
	[GeoId] [nvarchar](50) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[StateName] [nvarchar](50) NOT NUll,
	[State] [nvarchar](2) null,
	[TotPop65PlusSexRatio] float NOT NULL,
	[TotPopRace] float NOT NULL,
	[TotPopOneRace] float NOT NULL,
	[TotPopTwoOrMoreRace] float NOT NULL,
	[TotPopUnder5] float NOT NULL,
	[TotPop5to9] float NOT NULL,
	[TotPop10to14] float NOT NULL,
	[TotPop15to19] float NOT NULL,
	[TotPop20to24] float NOT NULL,
	[TotPop25to34] float NOT NULL,
	[TotPop35to44] float NOT NULL,
	[TotPop45to54] float NOT NULL,
	[TotPop55to59] float NOT NULL,
	[TotPop60to64] float NOT NULL,
	[TotPop65to74] float NOT NULL,
	[TotPop75to84] float NOT NULL,
	[TotPop85Plus] float NOT NULL,
	[TotPopMedianAge] float NOT NULL,
	[CensusClass] nvarchar(20) null
) ON [PRIMARY]
GO


