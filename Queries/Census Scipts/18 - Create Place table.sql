USE [Geography]
GO

/****** Object:  Table [dbo].[Places]    Script Date: 9/10/2020 12:37:08 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Places]') AND type in (N'U'))
DROP TABLE [dbo].[Places]
GO

/****** Object:  Table [dbo].[Places]    Script Date: 9/10/2020 12:37:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Places](
	[Id] int identity(0,1),
	[FipsId] [char](20) NULL,
	[GeoId] [nvarchar](7) NULL,
	[GNISCode] [nvarchar](8) null,
	[Name] [nvarchar](100) NULL,
	[LegalName] [nvarchar](100) NULL,
	[LSADId] [nvarchar](2) NULL,
	[FipsClass] [nvarchar](2) NULL,
	[MetroOrMicroIndicator] [nvarchar](1) NULL,
	[CensusPlace][bit] default(0) not null,
	[IncorporatedPlace][bit] default(0) not null,
	[AreaLand] [bigint] NULL,
	[AreaWater] [bigint] NULL,
	[Latitude] [float](53) NULL,
	[Longitude] [float](53) NULL,
	MinLatitude [float] null,
	MinLongitude [float] null,
	MaxLatitude [float] null,
	MaxLongitude [float] null,
	Shape Geography null
) ON [PRIMARY]
GO

-- run the shape and dbf importer now.


select * from dbo.Places