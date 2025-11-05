CREATE PROCEDURE dbo.P_DeleteAllClientOverallLimits
	
	(
	@companyID int
	)
	
AS
	DELETE FROM T_RMCompanyClientOverall
	WHERE        (CompanyID = @companyID)