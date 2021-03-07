USE [Geography]
GO

/****** Object:  Table [dbo].[Roads]    Script Date: 3/6/2021 3:27:58 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roads]') AND type in (N'U'))
DROP TABLE [dbo].[Roads]
GO

/****** Object:  Table [dbo].[Roads]    Script Date: 3/6/2021 3:27:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roads](
	[StCtyLinId] nvarchar(27) not null,
	[LinearId] [nvarchar](22) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[RouteType] [nvarchar](1) NULL,
	[MafFeatureCode] [nvarchar](5) NULL,
	[MinLatitude] [float] NULL,
	[MinLongitude] [float] NULL,
	[MaxLatitude] [float] NULL,
	[MaxLongitude] [float] NULL,
	[Shape] [geography] NULL,
	[FipsId] [char](20) null,
 CONSTRAINT [PK_Roads] PRIMARY KEY CLUSTERED 
(
	[StCtyLinId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


