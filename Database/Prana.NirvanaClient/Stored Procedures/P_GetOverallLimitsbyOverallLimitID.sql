

CREATE PROCEDURE dbo.P_GetOverallLimitsbyOverallLimitID
	(
		@companyID int,
		@rMCompanyOverallLimitID int
	)
AS
	SELECT     RMCompanyOverallLimitID, T_RMCompanyOverallLimit.CompanyID, RMBaseCurrencyID, CalculateRiskLimit, ExposureLimit, PositivePNL, NegativePNL
	FROM         T_RMCompanyOverallLimit 
	
	INNER JOIN T_Currency ON T_Currency.CurrencyID = T_RMCompanyOverallLimit.RMBaseCurrencyID
	INNER JOIN T_Company ON T_Company.CompanyID = T_RMCompanyOverallLimit.CompanyID 
	
	WHERE     T_RMCompanyOverallLimit.CompanyID = @companyID or RMCompanyOverallLimitID = @rMCompanyOverallLimitID    


