USE [Geography]
GO

/****** Object:  Table [dbo].[Roads]    Script Date: 1/24/2021 8:53:54 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roads]') AND type in (N'U'))
DROP TABLE [dbo].[Roads]
GO

/****** Object:  Table [dbo].[Roads]    Script Date: 1/24/2021 8:53:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roads](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinearId] [nvarchar](22) NULL,
	[FullName] [nvarchar](100) NULL,
	[RouteType] [nvarchar](1) NULL,
	[MafFeatureCode] [nvarchar](5) NULL,
	Shape [Geography] null
) ON [PRIMARY]
GO

