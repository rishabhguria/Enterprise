/****** Object:  Stored Procedure dbo.P_DeleteAllocationTradingAccountsUser    Script Date: 04/17/2006 7:20:24 PM ******/
CREATE PROCEDURE dbo.P_DeleteAllocationTradingAccountsUser
	(
		@companyUserID int
	)
AS
	Delete T_CompanyUserAllocationTradingAccounts
	Where CompanyUserID = @companyUserID


