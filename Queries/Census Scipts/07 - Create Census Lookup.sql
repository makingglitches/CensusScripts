USE [Geography]
GO

/****** Object:  Table [dbo].[CensusLookup]    Script Date: 9/6/2020 1:49:40 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CensusLookup]') AND type in (N'U'))
DROP TABLE [dbo].[CensusLookup]
GO

/****** Object:  Table [dbo].[CensusLookup]    Script Date: 9/6/2020 1:49:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CensusLookup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CensusColumn] [varchar](100) NOT NULL,
	[CensusDescription] [varchar](300) NOT NULL,
	[TableColumn] [varchar](50) NOT NULL
) ON [PRIMARY]
GO


