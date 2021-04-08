
use Geography
go


INSERT INTO [dbo].[GNISCodes]
           ([FEATURE_ID]
           ,[FEATURE_NAME]
           ,[FEATURE_CLASS]
           ,[STATE_ALPHA]
           ,[STATE_NUMERIC]
           ,[COUNTY_NAME]
           ,[COUNTY_NUMERIC]
           ,[PRIMARY_LAT_DMS]
           ,[PRIM_LONG_DMS]
           ,[PRIM_LAT_DEC]
           ,[PRIM_LONG_DEC]
           ,[SOURCE_LAT_DMS]
           ,[SOURCE_LONG_DMS]
           ,[SOURCE_LAT_DEC]
           ,[SOURCE_LONG_DEC]
           ,[ELEV_IN_M]
           ,[ELEV_IN_FT]
           ,[MAP_NAME]
           ,[DATE_CREATED]
           ,[DATE_EDITED]) 

SELECT [FEATURE_ID]
      ,[FEATURE_NAME]
      ,[FEATURE_CLASS]
      ,TRIM( [STATE_ALPHA])
      ,[STATE_NUMERIC]
      ,[COUNTY_NAME]
      ,[COUNTY_NUMERIC]
      ,[PRIMARY_LAT_DMS]
      ,[PRIM_LONG_DMS]
      ,[PRIM_LAT_DEC]
      ,[PRIM_LONG_DEC]
      ,[SOURCE_LAT_DMS]
      ,[SOURCE_LONG_DMS]
      ,[SOURCE_LAT_DEC]
      ,[SOURCE_LONG_DEC]
      ,[ELEV_IN_M]
      ,[ELEV_IN_FT]
      ,[MAP_NAME]
      ,[DATE_CREATED]
      ,[DATE_EDITED]


from 

openrowset(
BULK 'C:\Users\John\Documents\CensusProject\CensusShapeFileData\GNISNationalFile\NationalFile_20210101.txt',
FIRSTROW=2,
FORMATFILE='C:\Users\John\Documents\CensusProject\QrCode\GNISCodes.fmt'
--,ERRORFILE='C:\Users\John\Documents\CensusProject\QrCode\Input\Snap\Errors.log'
)t1