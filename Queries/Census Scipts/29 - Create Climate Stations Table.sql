USE [Geography]
GO

/****** Object:  Table [dbo].[ClimateStations]    Script Date: 2/6/2021 9:02:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClimateStations]') AND type in (N'U'))
DROP TABLE [dbo].[ClimateStations]
GO

/****** Object:  Table [dbo].[ClimateStations]    Script Date: 2/6/2021 9:02:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClimateStations](
	[Elevation] [float] NULL,
	[MinDate] [date] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Name] [nvarchar](250) NULL,
	[Stationid] [nvarchar](50) NULL,
	[ElevationUnit] [nvarchar](50) NULL
) ON [PRIMARY]
GO


