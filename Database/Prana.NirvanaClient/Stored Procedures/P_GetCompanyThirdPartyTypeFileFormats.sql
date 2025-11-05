CREATE PROCEDURE dbo.P_GetCompanyThirdPartyTypeFileFormats (@thirdPartyTypeId INT = 0)
AS
SELECT FileFormatId
	,FileFormatName
	,T_ThirdPartyFileFormat.ThirdPartyID
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
INNER JOIN T_ThirdParty ON T_ThirdPartyFileFormat.ThirdPartyId = T_ThirdParty.ThirdPartyID
LEFT OUTER JOIN T_FileData AS A ON T_ThirdPartyFileFormat.NirvanaToThirdPartyID = A.FileID
LEFT OUTER JOIN T_FileData AS C ON T_ThirdPartyFileFormat.HeaderFileID = C.FileID
LEFT OUTER JOIN T_FileData AS D ON T_ThirdPartyFileFormat.FooterFileID = D.FileID
WHERE T_ThirdParty.ThirdPartyID = @thirdPartyTypeId
ORDER BY FileFormatName
