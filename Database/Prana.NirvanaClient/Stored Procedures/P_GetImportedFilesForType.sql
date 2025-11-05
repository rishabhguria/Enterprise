CREATE PROCEDURE [dbo].[P_GetImportedFilesForType]
(
	@ImportType NVARCHAR (MAX)
)
AS
	Select ImportFileName From T_ImportFileLog
	WHERE ImportType = @ImportType
	AND (DATEDIFF(d, UTCTime, GETUTCDATE()) = 0)
