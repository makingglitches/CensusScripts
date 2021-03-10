USE [Geography]
GO

/****** Object:  Table [dbo].[Species]    Script Date: 3/9/2021 7:36:28 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Species]') AND type in (N'U'))
DROP TABLE [dbo].[Species]
GO

/****** Object:  Table [dbo].[Species]    Script Date: 3/9/2021 7:36:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Species](
	[DownloadLink] [nvarchar](300) NULL,
	[CommonName] [nvarchar](100) NULL,
	[Kingdom] [nvarchar](50) NULL,
	[ScientificName] [nvarchar](100) NULL,
	[ArchiveName] [nvarchar](200) NULL
) ON [PRIMARY]
GO


