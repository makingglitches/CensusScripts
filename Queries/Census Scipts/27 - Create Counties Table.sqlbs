USE [Geography]
GO

/****** Object:  Table [dbo].[Counties]    Script Date: 3/6/2021 7:16:44 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Counties]') AND type in (N'U'))
DROP TABLE [dbo].[Counties]
GO

/****** Object:  Table [dbo].[Counties]    Script Date: 3/6/2021 7:16:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Counties](
	[FipsId] [char](20) NULL,
	[GeoId] [char](5) NULL,
	[GNISId] [char](8) NOT NULL,
	[Name] [char](100) NULL,
	[NameLSAD] [char](100) NULL,
	[LSAD] [nchar](2) NULL,
	[ClassFP] [nchar](2) NULL,
	[MTFCC] [char](5) NULL,
	[AreaLand] [bigint] NULL,
	[AreaWater] [bigint] NULL,
	[Longitude] [float] NULL,
	[Latititude] [float] NULL,
	[MinLatitude] [float] NULL,
	[MinLongitude] [float] NULL,
	[MaxLatitude] [float] NULL,
	[MaxLongitude] [float] NULL,
	[Shape] [geography] NULL,
 CONSTRAINT [PK_Counties] PRIMARY KEY CLUSTERED 
(
	[GNISId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


