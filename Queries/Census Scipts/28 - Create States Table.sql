USE [Geography]
GO

/****** Object:  Table [dbo].[States]    Script Date: 2/6/2021 1:12:50 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[States]') AND type in (N'U'))
DROP TABLE [dbo].[States]
GO

/****** Object:  Table [dbo].[States]    Script Date: 2/6/2021 1:12:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[States](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionCode] [nchar](2) NULL,
	[DivisionCode] [nchar](2) NULL,
	[FipsKey] [nchar](2) NULL,
	[GNISKey] [nchar](8) NULL,
	[Abbreviation] [nchar](2) NULL,
	[Name] [nchar](100) NULL,
	[LSAD] [nchar](2) NULL,
	[AreaLand] [bigint] NULL,
	[AreaWater] [bigint] NULL,
	[Longitude] [float](11) NULL,
	[Latitude] [float](12) NULL,
	[Shape] Geography null
) ON [PRIMARY]
GO


