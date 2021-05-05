USE [Geography]
GO

/****** Object:  StoredProcedure [dbo].[GetKeyColumns]    Script Date: 3/8/2021 4:13:30 PM ******/
DROP PROCEDURE [dbo].[GetKeyColumns]
GO

/****** Object:  StoredProcedure [dbo].[GetKeyColumns]    Script Date: 3/8/2021 4:13:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		John R Sohn
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetKeyColumns] 
	-- Add the parameters for the stored procedure here
	@TableName nvarchar(Max) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dt.TABLE_NAME, dc.COLUMN_NAME, dc.DATA_TYPE from INFORMATION_SCHEMA.TABLES dt
	inner join INFORMATION_SCHEMA.COLUMNS dc 
	on dc.TABLE_NAME=dt.TABLE_NAME
	inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
	on kcu.TABLE_NAME = dt.TABLE_NAME and kcu.COLUMN_NAME = dc.COLUMN_NAME
	inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tcu
	on tcu.CONSTRAINT_NAME=kcu.CONSTRAINT_NAME
	where tcu.CONSTRAINT_TYPE='PRIMARY KEY' AND DT.TABLE_NAME=@TableName

END
GO


