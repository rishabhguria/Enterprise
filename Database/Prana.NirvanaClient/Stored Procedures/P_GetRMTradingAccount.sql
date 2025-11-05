CREATE PROCEDURE dbo.P_GetRMTradingAccount
	(
	  @companyID int,
	  @tradingAccountID int
	)
AS
	SELECT         CompanyTradAccntRMID, CompanyID, CompanyTradAccntID,  TAExposureLimit , TAPositivePNL, TANegativePNL
	FROM            T_RMCompanyTradAccntOverall
	WHERE        (CompanyTradAccntID = @tradingaccountID) AND (CompanyID = @companyID)      
