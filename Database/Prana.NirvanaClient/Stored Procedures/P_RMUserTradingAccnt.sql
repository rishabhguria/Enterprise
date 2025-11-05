CREATE PROCEDURE dbo.P_RMUserTradingAccnt 
	
	(
	@companyID int ,
	@tradingAccntID int,
	@companyUserID int
	
	)
	
AS
	SELECT        CompanyID, CompanyUserID, UserTradingAccntID, UserTAExposureLimit, RMUserTradingAccntID
	FROM            T_RMUserTradingAccount
	WHERE        (CompanyID = @companyID) AND (CompanyUserID = @companyUserID) AND (UserTradingAccntID = @tradingAccntID)
