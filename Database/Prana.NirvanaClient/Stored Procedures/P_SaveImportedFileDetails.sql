CREATE PROCEDURE P_SaveImportedFileDetails
(
	@importFileID INT OUTPUT,
	@importFileName nvarchar(500),
	@importFilePath nvarchar(max),
	@importType nvarchar(max),
	@importFIleLastModifiedTime datetime
)
AS 
BEGIN
INSERT INTO T_ImportFileLog (ImportFileName, ImportFilePath, ImportType, ImportFileLastModifiedUTCTime)
Values(@importFileName, @importFilePath, @importType, @importFIleLastModifiedTime)

SET @importFileID = @@IDENTITY
END
