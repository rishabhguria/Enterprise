CREATE PROCEDURE dbo.P_GetUserTradingAccounts (@userID INT)
AS
	IF @userID<0
	BEGIN
		SELECT CompanyTradingAccountsID, TradingAccountName FROM T_CompanyTradingAccounts 
	END
	ELSE
	BEGIN
		SELECT UTA.CompanyTradingAccountsID, UTA.TradingAccountName
		FROM T_CompanyTradingAccounts UTA INNER JOIN T_CompanyUserTradingAccounts TA
		ON UTA.CompanyTradingAccountsID = TA.TradingAccountID
		WHERE (TA.CompanyUserID = @userID)
	END
