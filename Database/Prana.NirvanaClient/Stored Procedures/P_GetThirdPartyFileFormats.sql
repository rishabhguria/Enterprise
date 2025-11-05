CREATE PROCEDURE [dbo].[P_GetThirdPartyFileFormats] (@thirdPartyId INT = 0)
AS
SELECT FileFormatId
	,FileFormatName
	,ThirdPartyId
	,A.FileNames AS NirvanaToThirdParty
	,C.FileNames AS HeaderFile
	,D.FileNames AS FooterFile
	,ExportOnly
	,Delimiter
	,DelimiterName
	,FileExtension
	,FileDisplayName
	,SPName
	,DoNotShowFileOpenDialogue
	,ClearExternalTransID
	,IncludeExercisedAssignedTransaction
	,IncludeExercisedAssignedUnderlyingTransaction
	,IncludeCATransaction
	,GenerateCancelNewForAmend
	,FIXEnabled
    ,FileEnabled
    ,FIXStorProc
	,TimeBatchesEnabled
FROM T_ThirdPartyFileFormat
LEFT OUTER JOIN T_FileData AS A ON T_ThirdPartyFileFormat.NirvanaToThirdPartyID = A.FileID
LEFT OUTER JOIN T_FileData AS C ON T_ThirdPartyFileFormat.HeaderFileID = C.FileID
LEFT OUTER JOIN T_FileData AS D ON T_ThirdPartyFileFormat.FooterFileID = D.FileID
WHERE ThirdPartyId = @thirdPartyId
ORDER BY FileFormatName
