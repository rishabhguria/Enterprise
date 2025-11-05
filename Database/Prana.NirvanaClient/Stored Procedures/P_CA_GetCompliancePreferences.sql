CREATE PROCEDURE [dbo].[P_CA_GetCompliancePreferences]
@CompanyId	INT
AS
BEGIN
	SELECT	CompanyId, 
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
	FROM	T_CA_CompliancePreferences 
	WHERE	@CompanyId = CompanyId
END



