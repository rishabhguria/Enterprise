/****** Object:  Stored Procedure dbo.P_GetCompanyThirdPartyFundCommissionRules    Script Date: 04/06/2006 12:50:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyThirdPartyFundCommissionRules
	(
		@companyID	int	
	)
AS
	
	 Select  CF.CompanyFundID, CF.FundName, CTPCR.CompanyCounterPartyCVID, CTPCR.CVAUECID, CTPCR.SingleRuleID, CTPCR.BasketRuleID
		FROM T_CompanyFunds CF inner join T_CompanyThirdPartyCVCommissionRules CTPCR on
			CF.CompanyFundID = CTPCR.CompanyFundID
				Where CompanyID = @companyID
				AND CF.IsActive=1 
				
				


