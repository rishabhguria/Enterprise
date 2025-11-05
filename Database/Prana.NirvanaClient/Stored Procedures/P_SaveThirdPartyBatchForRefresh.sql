CREATE PROCEDURE [dbo].[P_SaveThirdPartyBatchForRefresh]
	@Description nvarchar(50),
	@ThirdPartyTypeId int,
	@ThirdPartyId int,
	@ThirdPartyFormatId int,
	@IsLevel2Data bit,
	@Active bit,
	@AllowedFixTransmission bit = NULL, 
	@ThirdPartyCompanyId int
AS
BEGIN
	IF EXISTS (SELECT TOP 1 1 FROM T_ThirdPartyBatch WHERE ThirdPartyFormatId = @ThirdPartyFormatId AND ThirdPartyCompanyId = @ThirdPartyCompanyId AND ThirdPartyId = @ThirdPartyId AND ThirdPartyTypeId = @ThirdPartyTypeId)
	BEGIN
		Update T_ThirdPartyBatch
		Set AllowedFixTransmission = @AllowedFixTransmission
		WHERE ThirdPartyFormatId = @ThirdPartyFormatId
		AND ThirdPartyCompanyId = @ThirdPartyCompanyId
		AND ThirdPartyId = @ThirdPartyId
		AND ThirdPartyTypeId = @ThirdPartyTypeId
	END
	ELSE
	BEGIN
		Insert into T_ThirdPartyBatch ([Description], ThirdPartyTypeId, ThirdPartyId, ThirdPartyFormatId, IsLevel2Data, Active, ThirdPartyCompanyId, AllowedFixTransmission) 
		Values(@Description,@ThirdPartyTypeId,@ThirdPartyId,@ThirdPartyFormatId,@IsLevel2Data,@Active,@ThirdPartyCompanyId,@AllowedFixTransmission)
	END
END