CREATE PROCEDURE [dbo].[P_CA_SaveCompliancePreference]
(
@CompanyId			INT,
@ImportExportPath	VARCHAR(MAX),
@PrePostCrossImport	BIT,
@InMarket			BIT,
@InStage			BIT,
@PostInMarket		BIT,
@PostInStage		BIT,
@BlockTradeOnComplianceFaliure		BIT,
@StageValueFromField		BIT,
@StageValueFromFieldString VARCHAR(100),
@IsBasketComplianceEnabledCompany  BIT
)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM T_CA_CompliancePreferences WHERE CompanyId = @CompanyId)
	BEGIN
		INSERT INTO T_CA_CompliancePreferences
		(
			CompanyId,
			ImportExportPath,
			PrePostCrossImport,
			InMarket,
			InStage,
			PostInMarket,
			PostInStage,
			BlockTradeOnComplianceFaliure,
			StageValueFromField,
			StageValueFromFieldString,
			IsBasketComplianceEnabledCompany
		)
		VALUES
		(
			@CompanyId,
			@ImportExportPath,
			@PrePostCrossImport,
			@InMarket,
			@InStage,
			@PostInMarket,
			@PostInStage,
			@BlockTradeOnComplianceFaliure,
			@StageValueFromField,
			@StageValueFromFieldString,
			@IsBasketComplianceEnabledCompany
		)
	END
	ELSE
	BEGIN
		UPDATE	T_CA_CompliancePreferences
		SET		ImportExportPath		= @ImportExportPath,
				PrePostCrossImport		= @PrePostCrossImport,
				InMarket				= @InMarket,
				InStage					= @InStage,
				PostInMarket			= @PostInMarket,
				PostInStage				= @PostInStage,
				BlockTradeOnComplianceFaliure= @BlockTradeOnComplianceFaliure,
				StageValueFromField =@StageValueFromField,
				StageValueFromFieldString =@StageValueFromFieldString,
				IsBasketComplianceEnabledCompany=@IsBasketComplianceEnabledCompany


		WHERE	CompanyId				= @CompanyId
	END	
	IF((select IsBasketComplianceEnabledCompany from T_CA_CompliancePreferences where CompanyId=@CompanyId)=0)
	BEGIN 
	          UPDATE T_CA_OtherCompliancePermission
	          SET EnableBasketComplianceCheck=0 
			  Where CompanyId=@CompanyId
    END
END	


