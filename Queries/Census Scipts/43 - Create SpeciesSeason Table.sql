USE [Geography]
GO

/****** Object:  Table [dbo].[SpeciesSeason]    Script Date: 3/11/2021 7:18:53 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpeciesSeason]') AND type in (N'U'))
DROP TABLE [dbo].[SpeciesSeason]
GO

/****** Object:  Table [dbo].[SpeciesSeason]    Script Date: 3/11/2021 7:18:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SpeciesSeason](
	[DownloadGuid] [nvarchar](100) NOT NULL,
	[Season] [int] NOT NULL,
	[SeasonName] [nvarchar](254) NULL,
	[MinLatitude] float null,
	[MinLongitude] float null,
	[MaxLatitude] float null,
	[MaxLongitude] float null,
 CONSTRAINT [PK_SpeciesSeason] PRIMARY KEY CLUSTERED 
(
	[DownloadGuid] ASC,
	[Season] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


