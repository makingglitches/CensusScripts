USE [Geography]
GO

/****** Object:  Table [dbo].[store]    Script Date: 9/1/2020 1:16:43 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[store]') AND type in (N'U'))
DROP TABLE [dbo].[store]
GO

/****** Object:  Table [dbo].[store]    Script Date: 9/1/2020 1:16:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[store](
	[text] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

declare @json as nvarchar(max)

SELECT @json= BulkColumn
 FROM OPENROWSET (BULK 'C:\Users\Rescue\Documents\QrCode\Input\us-zip-code-latitude-and-longitude.json', SINGLE_CLOB) as j;

 insert into dbo.store(text) values (@json)

 GO



/*
{"datasetid": "us-zip-code-latitude-and-longitude", 
 "recordid": "6a0a9c66f8e0292a54c9f023c93732f1b41d8943", 
 "fields": 
		{"city": 
		 "Cove", 
		 "zip": "71937", 
		 "dst": 1, 
		 "geopoint": [34.398483, -94.39398], 
		 "longitude": -94.39398, 
		 "state": "AR", 
		 "latitude": 34.398483, 
		 "timezone": -6}, 
 "geometry": {"type": "Point", "coordinates": [-94.39398, 34.398483]}, 
 
 "record_timestamp": "2018-02-09T09:33:38.603-07:00"}

 if this doesnt look familiar dont know what does

 only actually need a couple fields from this the other project can pretty much be scrapped since json is native to 
 web but that doesnt help if a server side component is being written in a REAL language.

 may use a web interface for display since john r sohn,  knows that svg is useful enough... though there is still some question as to why to do this 
 since browsers dont always display things as they do.
 electron looks like a decent technology for that purpose.
 */

 