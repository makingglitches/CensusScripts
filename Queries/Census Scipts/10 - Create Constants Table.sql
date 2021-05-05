USE [Geography]
GO

/****** Object:  Table [dbo].[Constants]    Script Date: 9/7/2020 7:24:39 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Constants]') AND type in (N'U'))
DROP TABLE [dbo].[Constants]
GO

/****** Object:  Table [dbo].[Constants]    Script Date: 9/7/2020 7:24:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Constants](
	[ConstantName] [varchar](50) NOT NULL,
	[Value] [real] NOT NULL,
 CONSTRAINT [PK_Constants] PRIMARY KEY CLUSTERED 
(
	[ConstantName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


