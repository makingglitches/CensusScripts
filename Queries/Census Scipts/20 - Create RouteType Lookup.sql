USE [Geography]
GO

/****** Object:  Table [dbo].[RouteTypeLookup]    Script Date: 1/24/2021 8:56:20 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RouteTypeLookup]') AND type in (N'U'))
DROP TABLE [dbo].[RouteTypeLookup]
GO

/****** Object:  Table [dbo].[RouteTypeLookup]    Script Date: 1/24/2021 8:56:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RouteTypeLookup](
	[RouteTypeCode] [nvarchar](1) NULL,
	[Description] [nvarchar](200) NULL
) ON [PRIMARY]
GO


