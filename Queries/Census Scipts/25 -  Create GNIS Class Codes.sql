USE [Geography]
GO

/****** Object:  Table [dbo].[GNISClasses]    Script Date: 2/3/2021 7:35:24 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GNISClasses]') AND type in (N'U'))
DROP TABLE [dbo].[GNISClasses]
GO

/****** Object:  Table [dbo].[GNISClasses]    Script Date: 2/3/2021 7:35:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GNISClasses](
	[Class] [nvarchar](50) NULL,
	[Description] [nvarchar](1024) NULL
) ON [PRIMARY]
GO


