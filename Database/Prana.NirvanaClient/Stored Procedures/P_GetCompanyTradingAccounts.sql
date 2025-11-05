CREATE PROCEDURE dbo.P_GetCompanyTradingAccounts (@companyID INT)
AS
SELECT CompanyTradingAccountsID
	,TradingAccountName
	,TradingShortName
	,CompanyID
FROM T_CompanyTradingAccounts
WHERE CompanyID = @companyID
