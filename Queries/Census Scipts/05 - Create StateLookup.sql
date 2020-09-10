USE [Geography]
GO

/****** Object:  Table [dbo].[StateLookup]    Script Date: 9/6/2020 7:37:27 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StateLookup]') AND type in (N'U'))
DROP TABLE [dbo].[StateLookup]
GO

/****** Object:  Table [dbo].[StateLookup]    Script Date: 9/6/2020 7:37:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StateLookup](
	[StateName] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Fips] [nvarchar](2) null
) ON [PRIMARY]
GO


