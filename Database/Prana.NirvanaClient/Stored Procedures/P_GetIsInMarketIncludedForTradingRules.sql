CREATE PROCEDURE [dbo].[P_GetIsInMarketIncludedForTradingRules]
	@companyID int 
AS
	SELECT IsInMarketIncluded FROM T_TradingRulesPreferences WHERE CompanyID = @companyID

