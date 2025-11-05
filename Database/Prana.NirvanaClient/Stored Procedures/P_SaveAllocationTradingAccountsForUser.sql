
/****** Object:  Stored Procedure dbo.P_SaveAllocationTradingAccountsForUser    Script Date: 04/17/2006 7:15:24 PM ******/
CREATE PROCEDURE dbo.P_SaveAllocationTradingAccountsForUser
	(
		@companyTradingAccountID int,
		@companyUserID int
	)
AS

	Insert T_CompanyUserAllocationTradingAccounts(CompanyTradingAccountID, CompanyUserID)
	Values(@companyTradingAccountID, @companyUserID)
	
	

