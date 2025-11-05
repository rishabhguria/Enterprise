-- =============================================
-- Author:		Sumit Kakra
-- Create date: November 23, 2006
-- Description:	Imports Diagrams from source DB
--				Example Exec ImportDiagrams 'NirvanaClient', 'PM%'
-- =============================================
CREATE PROCEDURE ImportDiagrams 
	-- Add the parameters for the stored procedure here
	@SourceDB nvarchar(50) = 'NirvanaClient', 
	@Filter nvarchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	EXEC('	INSERT INTO [dbo].[sysdiagrams]
		([name],[principal_id],[version],[definition])
			SELECT [name],[principal_id],[version],[definition]
			FROM ' + @SourceDB + '.[dbo].[sysdiagrams] where name like' + @Filter)
END
