
/*
	Name : <P_TPGetFundCommissionRule>
	Created by : <kanupriya>
	Dated : <11/03/2006>
	Purpose: <To fetch the commission rule applicable on a given set of fundID, cvID, auecID for a trade.>
*/
CREATE PROCEDURE dbo.P_TPGetFundCommissionRule
	(
	@companyFundID int ,
	@cVID int,
	@auecID int
	)
	
AS
	SELECT     T_CompanyThirdPartyCVCommissionRules.CompanyFundID, T_CompanyFunds.FundShortName, T_CompanyThirdPartyCVCommissionRules.CompanyCounterPartyCVID, 
	                      T_CompanyThirdPartyCVCommissionRules.CVAUECID, T_CompanyThirdPartyCVCommissionRules.SingleRuleID, 
	                      T_CompanyThirdPartyCVCommissionRules.BasketRuleID
	FROM         T_CompanyThirdPartyCVCommissionRules INNER JOIN
	                      T_CompanyFunds ON T_CompanyThirdPartyCVCommissionRules.CompanyFundID = T_CompanyFunds.CompanyFundID
	WHERE     (T_CompanyThirdPartyCVCommissionRules.CompanyFundID = @companyFundID) AND 
	                      (T_CompanyThirdPartyCVCommissionRules.CompanyCounterPartyCVID = @cVID) AND 
	                      (T_CompanyThirdPartyCVCommissionRules.CVAUECID = @auecID)
