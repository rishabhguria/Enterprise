CREATE PROCEDURE dbo.P_DeleteFundAccount
	
	(
	@companyID int ,
	@fundAccntID int
	)
	
AS
	DELETE FROM T_RMCompanyFundAccntOverall
	WHERE        (CompanyID = @companyID) AND (CompanyFundAccntID = @fundAccntID)
