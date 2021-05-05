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

-- if anyone other than john r sohn has this they're a fucking pedophile piece
-- of garbage still pretending the year is before 2021
-- we are not supposed to be stuck here, never were and they have been
-- stealing our things previously to replace us with some piece
-- of chomo garbage and creating excuses to keep everyone fooled who
-- isnt one of them.

insert into dbo.RouteTypeLookup( RouteTypeCode,Description)
values 
('C','County'),
('I','Interstate'),
('M','Common Name'),
('O','Other'),
('S','State recognized'),
('U','U.S.')

GO