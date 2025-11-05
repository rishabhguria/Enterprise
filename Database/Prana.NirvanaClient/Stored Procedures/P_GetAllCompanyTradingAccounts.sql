


/****** Object:  Stored Procedure dbo.P_GetAllCompanyTradingAccounts    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetAllCompanyTradingAccounts
AS
	SELECT   CompanyTradingAccountsID, TradingAccountName, TradingShortName, CompanyID
FROM         T_CompanyTradingAccounts



