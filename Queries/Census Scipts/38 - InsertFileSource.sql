USE [Geography]
GO

/****** Object:  StoredProcedure [dbo].[InsertFileSource]    Script Date: 3/8/2021 9:30:19 PM ******/
DROP PROCEDURE [dbo].[InsertFileSource]
GO

/****** Object:  StoredProcedure [dbo].[InsertFileSource]    Script Date: 3/8/2021 9:30:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		John R Sohn
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[InsertFileSource] 
	-- Add the parameters for the stored procedure here
	@filename nvarchar(300) ='' ,
	@filesize int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if ( (select count(*) from dbo.FileSources where FileName=@filename) = 0)
	begin
		insert into dbo.FileSources(Active,FileName,Purpose,SizeBytes)
		values (1,@filename,null, @filesize)
	end
END
GO


