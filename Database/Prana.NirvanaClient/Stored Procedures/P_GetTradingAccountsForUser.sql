



/****** Object:  Stored Procedure dbo.P_GetTradingAccountsForUser    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE [dbo].[P_GetTradingAccountsForUser]
	(
		@userID int
	)
AS
SELECT     T_CompanyTradingAccounts.CompanyTradingAccountsID, T_CompanyTradingAccounts.TradingAccountName, 
			   T_CompanyTradingAccounts.TradingShortName, T_CompanyTradingAccounts.CompanyID
	FROM       T_CompanyUserTradingAccounts INNER JOIN
               T_CompanyTradingAccounts ON 
               T_CompanyUserTradingAccounts.TradingAccountID = T_CompanyTradingAccounts.CompanyTradingAccountsID 
Where          T_CompanyUserTradingAccounts.CompanyUserID = @userID and T_CompanyTradingAccounts.IsActive=1

