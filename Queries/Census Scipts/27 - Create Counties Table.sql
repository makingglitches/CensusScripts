USE [Geography]
GO

/****** Object:  Table [dbo].[Counties]    Script Date: 2/3/2021 8:38:02 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Counties]') AND type in (N'U'))
DROP TABLE [dbo].[Counties]
GO

/****** Object:  Table [dbo].[Counties]    Script Date: 2/3/2021 8:38:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Counties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FipsId] [char](20) NULL,
	[GeoId] [char](5) NULL,
	[GNISId] [char](8) NULL,
	[Name] [char](100) NULL,
	[NameLSAD] [char](100) NULL,
	[LSAD] [nchar](2) NULL,
	[ClassFP] [nchar](2) NULL,
	[MTFCC] [char](5) NULL,
	[AreaLand] [bigint] NULL,
	[AreaWater] [bigint] NULL,
	[Longitude] [float] NULL,
	[Latititude] [float] NULL,
	[Shape] [geography] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


