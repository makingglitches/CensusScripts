USE [Geography]
GO

/****** Object:  Table [dbo].[Rivers]    Script Date: 2/15/2021 12:40:15 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rivers]') AND type in (N'U'))
DROP TABLE [dbo].[Rivers]
GO

/****** Object:  Table [dbo].[Rivers]    Script Date: 2/15/2021 12:40:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rivers](
	[ObjectId] [int] Not NULL,
	[Name] [nvarchar](300) NULL,
	[StateAbbreviation] [nvarchar](8) NULL,
	[Region] [int] NULL,
	[Miles] [float] NULL,
	[ShapeLength] [float] NULL,
	Shape geography,
	MinLatitude float,
	MinLongitude float,
	MaxLatitude float,
	MaxLongitude float,

 CONSTRAINT [PK_Rivers] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


