USE [Geography]
GO

ALTER TABLE [dbo].[Places] DROP CONSTRAINT [DF__Places__Incorpor__02925FBF]
GO

ALTER TABLE [dbo].[Places] DROP CONSTRAINT [DF__Places__CensusPl__019E3B86]
GO

/****** Object:  Table [dbo].[Places]    Script Date: 3/6/2021 8:23:31 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Places]') AND type in (N'U'))
DROP TABLE [dbo].[Places]
GO

/****** Object:  Table [dbo].[Places]    Script Date: 3/6/2021 8:23:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Places](
	[FipsId] [char](20) NULL,
	[GeoId] [nvarchar](7) NULL,
	[GNISCode] [nvarchar](8) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[LegalName] [nvarchar](100) NULL,
	[LSADId] [nvarchar](2) NULL,
	[FipsClass] [nvarchar](2) NULL,
	[MetroOrMicroIndicator] [nvarchar](1) NULL,
	[CensusPlace] [bit] NOT NULL,
	[IncorporatedPlace] [bit] NOT NULL,
	[AreaLand] [bigint] NULL,
	[AreaWater] [bigint] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[MinLatitude] [float] NULL,
	[MinLongitude] [float] NULL,
	[MaxLatitude] [float] NULL,
	[MaxLongitude] [float] NULL,
	[Shape] [geography] NULL,
 CONSTRAINT [PK_Places] PRIMARY KEY CLUSTERED 
(
	[GNISCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Places] ADD  DEFAULT ((0)) FOR [CensusPlace]
GO

ALTER TABLE [dbo].[Places] ADD  DEFAULT ((0)) FOR [IncorporatedPlace]
GO


