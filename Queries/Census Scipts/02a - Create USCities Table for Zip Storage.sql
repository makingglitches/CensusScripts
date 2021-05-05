USE [Geography]
GO

ALTER TABLE [dbo].[USCities] DROP CONSTRAINT [DF_USCities_Added]
GO

/****** Object:  Table [dbo].[USCities]    Script Date: 3/8/2021 4:42:52 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USCities]') AND type in (N'U'))
DROP TABLE [dbo].[USCities]
GO

/****** Object:  Table [dbo].[USCities]    Script Date: 3/8/2021 4:42:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USCities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Added] [datetime] NOT NULL,
	[recordid] [nvarchar](32) NOT NULL,
	[City] [nchar](60) NOT NULL,
	[State] [nchar](2) NOT NULL,
	[Zip] [nchar](5) NOT NULL,
	[Dst] [int] NOT NULL,
	[Longitude] [real] NOT NULL,
	[Latitude] [real] NOT NULL,
	[TimeZone] [int] NOT NULL,
	[RecordStamp] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_USCities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[USCities] ADD  CONSTRAINT [DF_USCities_Added]  DEFAULT (getdate()) FOR [Added]
GO


