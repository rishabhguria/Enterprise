CREATE PROCEDURE P_GetImportedFileDetails
(
	@dirPath nvarchar(max)
)
AS 
BEGIN
SELECT ImportFileName,ImportFileLastModifiedUTCTime FROM T_ImportFileLog
WHERE ImportFilePath LIKE @dirPath+'\%'
END
