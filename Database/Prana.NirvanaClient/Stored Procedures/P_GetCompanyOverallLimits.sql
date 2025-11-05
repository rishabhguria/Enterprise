

/****** Object:  Stored Procedure dbo.P_GetCompanyOverallLimits    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyOverallLimits
	(
		@companyID int
		
	)
AS
      select   RMCompanyOverallLimitID, T_RMCompanyOverallLimit.CompanyID, RMBaseCurrencyID,
				 CalculateRiskLimit, ExposureLimit, PositivePNL, NegativePNL
			FROM      T_RMCompanyOverallLimit 
	
	INNER JOIN T_Currency ON T_Currency.CurrencyID = T_RMCompanyOverallLimit.RMBaseCurrencyID
	INNER JOIN T_Company ON T_Company.CompanyID = T_RMCompanyOverallLimit.CompanyID 
	
	    




