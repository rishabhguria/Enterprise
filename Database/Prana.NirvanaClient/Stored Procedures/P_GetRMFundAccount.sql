CREATE PROCEDURE dbo.P_GetRMFundAccount
	
	(
		@companyID int,
		@companyFundAccntID int
	)
	
AS
	SELECT        CompanyFundAccntRMID, ExposureLimitRMBaseCurrency, CompanyID, FANegativePNL, FAPositivePNL, CompanyFundAccntID
	FROM            T_RMCompanyFundAccntOverall
	WHERE        (CompanyID = @companyID) AND (CompanyFundAccntID = @companyFundAccntID)
