CREATE PROCEDURE dbo.P_GetCompanyThirdPartyFileFormats (@thirdPartyId INT = 0)
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
INNER JOIN T_CompanyThirdParty ON T_ThirdPartyFileFormat.ThirdPartyId = T_CompanyThirdParty.ThirdPartyID
LEFT OUTER JOIN T_FileData AS A ON T_ThirdPartyFileFormat.NirvanaToThirdPartyID = A.FileID
LEFT OUTER JOIN T_FileData AS C ON T_ThirdPartyFileFormat.HeaderFileID = C.FileID
LEFT OUTER JOIN T_FileData AS D ON T_ThirdPartyFileFormat.FooterFileID = D.FileID
WHERE T_CompanyThirdParty.CompanyThirdPartyId = @thirdPartyId
ORDER BY FileFormatName
