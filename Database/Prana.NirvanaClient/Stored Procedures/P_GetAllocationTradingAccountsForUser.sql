
/****** Object:  Stored Procedure dbo.P_GetAllocationTradingAccountsForUser    Script Date: 04/17/2006 7:50:24 PM ******/
CREATE PROCEDURE dbo.P_GetAllocationTradingAccountsForUser
	(
		@companyUserID int
	)
AS
	SELECT     CUATA.CompanyTradingAccountID, CTA.TradingAccountName, 
				CTA.TradingShortName, CTA.CompanyID
	FROM         T_CompanyUserAllocationTradingAccounts CUATA INNER JOIN
                      T_CompanyTradingAccounts CTA ON 
                      CUATA.CompanyTradingAccountID = CTA.CompanyTradingAccountsID 
 Where CUATA.CompanyUserID = @companyUserID





