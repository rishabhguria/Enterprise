
/****** Object:  Stored Procedure dbo.P_DeleteSelectedFileFormat   Script Date: 16-Aug-2007 ******/
CREATE PROCEDURE [dbo].[P_DeleteSelectedFileFormat] (
	@ThirdPartyId INT
	,@FileFormatId INT
	)
AS
DECLARE @result INT
SET @result = 0
DECLARE @NirvanaToThirdPartyXSLTID INT
DECLARE @HeaderFileID INT
DECLARE @FooterFileID INT
DECLARE @NoOfTrades INT

SET @NoOfTrades = (
		SELECT count(*)
		FROM T_PBWiseTaxlotState PTS
		INNER JOIN T_CompanyThirdParty CTP ON PTS.PBID = CTP.CompanyThirdPartyID
		WHERE ThirdPartyId = @ThirdPartyId
			AND FileFormatID = @FileFormatId
			AND TaxlotState <> 0
		)

IF @NoOfTrades = 0
BEGIN
	SELECT @NirvanaToThirdPartyXSLTID = NirvanaToThirdPartyID
		,@HeaderFileID = HeaderFileID
		,@FooterFileID = FooterFileID
	FROM T_ThirdPartyFileFormat
	WHERE FileFormatId = @FileFormatId
		AND ThirdPartyId = @ThirdPartyId

	DELETE
	FROM T_ThirdPartyFileFormat
	WHERE FileFormatId = @FileFormatId
		AND ThirdPartyId = @ThirdPartyId

	IF (@NirvanaToThirdPartyXSLTID IS NOT NULL)
	BEGIN
		DELETE
		FROM T_FileData
		WHERE FileId = @NirvanaToThirdPartyXSLTID
	END

	IF (@HeaderFileID IS NOT NULL)
	BEGIN
		DELETE
		FROM T_FileData
		WHERE FileId = @HeaderFileID
	END

	IF (@FooterFileID IS NOT NULL)
	BEGIN
		DELETE
		FROM T_FileData
		WHERE FileId = @FooterFileID
	END

	DELETE
	FROM T_PBWiseTaxlotState
	WHERE PBID = @ThirdPartyId
		AND FileFormatId = @FileFormatId

	DELETE FROM T_ThirdPartyTimeBatches
	WHERE FileFormatId = @FileFormatId

	SET @result = 1
END

--End            
SELECT @result
