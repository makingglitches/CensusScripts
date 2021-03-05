USE [Geography]
GO

/****** Object:  Table [dbo].[Aquifer]    Script Date: 2/25/2021 7:43:20 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Aquifer]') AND type in (N'U'))
DROP TABLE [dbo].[Aquifer]
GO

/****** Object:  Table [dbo].[Aquifer]    Script Date: 2/25/2021 7:43:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Aquifer](
	[ObjectId] [int] not NULL,
	[RockName] [nvarchar](500) NULL,
	[RockType] [int] NULL,
	[AquiferName] [nvarchar](500) NULL,
	[AquiferCode] [int] NULL,
	[ShapeLength] [float] NULL,
	[ShapeArea] [float] NULL,
	[MinLatitude] [float] NULL,
	[MinLongitude] [float] NULL,
	[MaxLatitude] [float] NULL,
	[MaxLongitude] [float] NULL,
	[Shape] [geography] NULL,
 CONSTRAINT [PK_Aquifer] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


