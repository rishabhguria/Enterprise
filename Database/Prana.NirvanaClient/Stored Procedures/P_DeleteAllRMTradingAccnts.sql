CREATE PROCEDURE dbo.P_DeleteAllRMTradingAccnts
	
	(
	@companyID int 
	)
	
AS
	SELECT        CompanyTradAccntRMID, CompanyID, CompanyTradAccntID, TAExposureLimit, TANegativePNL, TAPositivePNL
	FROM            T_RMCompanyTradAccntOverall
	WHERE        (CompanyID = @companyID)
