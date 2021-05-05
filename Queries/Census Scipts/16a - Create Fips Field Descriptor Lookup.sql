USE [Geography]
GO

/****** Object:  Table [dbo].[FipsLookup]    Script Date: 9/10/2020 9:22:00 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FipsLookup]') AND type in (N'U'))
DROP TABLE [dbo].[FipsLookup]
GO

/****** Object:  Table [dbo].[FipsLookup]    Script Date: 9/10/2020 9:22:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FipsLookup](
	[FieldName] [nvarchar](50) NOT NULL,
	[FieldDescription] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO


insert into dbo.FipsLookup(FieldName,FieldDescription)
values
('SummLevel','Summary Level'),
('State','State Code (FIPS)'),
('County','County Code (FIPS)'),
('CountySubDivision','County Subdivision Code (FIPS)'),
('PlaceCode','Place Code (FIPS)'),
('ConsolidatedCity','Consolidtated City Code (FIPS)'),
('AreaName','Area Name (including legal/statistical area description)')

Go