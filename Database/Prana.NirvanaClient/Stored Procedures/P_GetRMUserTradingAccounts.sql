CREATE PROCEDURE dbo.P_GetRMUserTradingAccounts
	
	(
	@companyID int ,
	@userTradingAccountID int
	)
	
AS
	SELECT     RMUserTradingAccntID, CompanyID, CompanyUserID, UserTradingAccntID, UserTAExposureLimit
	FROM         T_RMUserTradingAccount
	WHERE     (CompanyID = @companyID) AND (UserTradingAccntID = @userTradingAccountID)
