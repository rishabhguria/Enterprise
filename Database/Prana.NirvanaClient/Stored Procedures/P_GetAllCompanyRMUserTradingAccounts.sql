CREATE PROCEDURE dbo.P_GetAllCompanyRMUserTradingAccounts
	
	(
	@companyID int
	)
	
AS
	SELECT     RMUserTradingAccntID, CompanyID, CompanyUserID, UserTradingAccntID, UserTAExposureLimit
	FROM         T_RMUserTradingAccount
	WHERE     (CompanyID = @companyID)