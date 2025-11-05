CREATE PROCEDURE dbo.P_DeleteAllRMFundAccounts
	
	(
	@companyID int 
	)
	
AS
	DELETE FROM T_RMCompanyFundAccntOverall
	WHERE        (CompanyID = @companyID)
