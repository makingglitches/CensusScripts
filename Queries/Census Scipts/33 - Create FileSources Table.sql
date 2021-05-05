USE [Geography]
GO

-- this is not particularily useful just this second
-- but will be in the future if needs be to track something down that isnt right for debugging purposes
-- making sure to add recordindex and filesourceid to every other table.

ALTER TABLE [dbo].[FileSources] DROP CONSTRAINT [DF_FileSources_Active]
GO

/****** Object:  Table [dbo].[FileSources]    Script Date: 3/4/2021 3:00:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileSources]') AND type in (N'U'))
DROP TABLE [dbo].[FileSources]
GO

/****** Object:  Table [dbo].[FileSources]    Script Date: 3/4/2021 3:00:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FileSources](
	[FileName] [varchar](300) not NULL,
	[Purpose] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[SizeBytes] bigint not null,
 CONSTRAINT [PK_FileSources] PRIMARY KEY CLUSTERED 
(
	[FileName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FileSources] ADD  CONSTRAINT [DF_FileSources_Active]  DEFAULT ((0)) FOR [Active]
GO



