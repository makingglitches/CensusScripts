USE [Geography]
GO

/****** Object:  Table [dbo].[LSADLookup]    Script Date: 9/13/2020 8:54:41 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LSADLookup]') AND type in (N'U'))
DROP TABLE [dbo].[LSADLookup]
GO

/****** Object:  Table [dbo].[LSADLookup]    Script Date: 9/13/2020 8:54:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LSADLookup](
	[Code] [nvarchar](50) not NULL,
	[Description] [nvarchar](100) not NULL,
	[AreaInformation] [nvarchar](max) not NULL,
	primary key(code)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


insert into dbo.LSADLookup(code,Description,AreaInformation)
SELECT T1.code,t1.description,t1.areainformation
      FROM  OPENROWSET(BULK  'C:\Users\John\Documents\QrCode\Input\Places2019\FormattedLSADCodes.csv',  
	  format='csv',
     FORMATFILE='C:\Users\John\Documents\QrCode\Queries\Census Scipts\LSADCodesFormat.fmt'    
       ) t1

go
