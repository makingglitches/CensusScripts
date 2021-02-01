USE [Geography]
GO

-- so.. seriously stop stealing these things or create a copy of them 
-- i can FUCKING FIND TO USE IN MY OWN GODDAMN PROJECTS THOUGH JOHN R SOHN WOULD 
-- PREFER TO HAVE THE CREDIT FOR DEVELOPING THE DATABASE AND IMPORT SCRIPTS !!

/****** Object:  Table [dbo].[GNISCodes]    Script Date: 1/31/2021 8:48:19 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GNISCodes]') AND type in (N'U'))
DROP TABLE [dbo].[GNISCodes]
GO

/****** Object:  Table [dbo].[GNISCodes]    Script Date: 1/31/2021 8:48:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GNISCodes](
	[FEATURE_ID] [int] NULL,
	[FEATURE_NAME] [nvarchar](120) NULL,
	[FEATURE_CLASS] [nvarchar](50) NULL,
	[STATE_ALPHA] [nvarchar](4) NULL,
	[STATE_NUMERIC] [nvarchar](4) NULL,
	[COUNTY_NAME] [nvarchar](100)  NULL,
	[COUNTY_NUMERIC] [nvarchar](3) NULL,
	[PRIMARY_LAT_DMS] [nvarchar](7) NULL,
	[PRIM_LONG_DMS] [nvarchar](8) NULL,
	[PRIM_LAT_DEC] [float] NULL,
	[PRIM_LONG_DEC] [float] NULL,
	[SOURCE_LAT_DMS] [nvarchar](7)  NULL,
	[SOURCE_LONG_DMS] [nvarchar](8)  NULL,
	[SOURCE_LAT_DEC] [float]  NULL,
	[SOURCE_LONG_DEC] [float]  NULL,
	[ELEV_IN_M] [float] NULL,
	[ELEV_IN_FT] [float] NULL,
	[MAP_NAME] [nvarchar](100) NULL,
	[DATE_CREATED] [date]  NULL,
	[DATE_EDITED] [date] NULL
) ON [PRIMARY]
GO


