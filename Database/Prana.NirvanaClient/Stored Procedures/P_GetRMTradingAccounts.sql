CREATE PROCEDURE dbo.P_GetRMTradingAccounts
	(
	  @companyID int

	)
AS
	SELECT        CompanyTradAccntRMID , CompanyID, CompanyTradAccntID, TAPositivePNL, TAExposureLimit, TANegativePNL
	FROM            T_RMCompanyTradAccntOverall
	WHERE        CompanyID = @companyID        
	
