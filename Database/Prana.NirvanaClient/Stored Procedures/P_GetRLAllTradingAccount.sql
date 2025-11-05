

CREATE PROCEDURE dbo.P_GetRLAllTradingAccount
(
		@CompanyID int
)
AS
SELECT DISTINCT CompanyTradingAccountsID, TradingAccountName
FROM         T_CompanyTradingAccounts
WHERE     (CompanyID = @CompanyID)
ORDER BY TradingAccountName


