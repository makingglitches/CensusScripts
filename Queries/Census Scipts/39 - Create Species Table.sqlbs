USE [Geography]
GO

/****** Object:  Table [dbo].[Species]    Script Date: 3/11/2021 1:53:16 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Species]') AND type in (N'U'))
DROP TABLE [dbo].[Species]
GO

/****** Object:  Table [dbo].[Species]    Script Date: 3/11/2021 1:53:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Species](
	[DownloadLink] [nvarchar](300) NOT NULL,
	[CommonName] [nvarchar](100) NOT NULL,
	[AnimalType] [nvarchar](50) NOT NULL,
	[ScientificName] [nvarchar](100) NOT NULL,
	[ServerArchiveName] [nvarchar](200) NOT NULL,
	[ArchiveName] [nvarchar](200) NOT NULL,
	[DownloadGuid] [nvarchar](100) NOT NULL,
	[ContentsPrefix] [nvarchar](50) NULL,
	[DescriptorXMLMatches] [bit] NULL,
 CONSTRAINT [PK_Species] PRIMARY KEY CLUSTERED 
(
	[DownloadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


