CREATE PROCEDURE dbo.P_GetRMFundAccounts 
	
	(
	
		@companyID int
	
	)
	
AS
	SELECT        CompanyFundAccntRMID, ExposureLimitRMBaseCurrency, CompanyID, FANegativePNL, FAPositivePNL, CompanyFundAccntID
	FROM            T_RMCompanyFundAccntOverall
	WHERE        (CompanyID = @companyID)