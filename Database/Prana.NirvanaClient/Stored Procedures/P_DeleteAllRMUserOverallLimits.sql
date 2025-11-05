CREATE PROCEDURE dbo.P_DeleteAllRMUserOverallLimits
	
	(
	@companyID int 
	)
	
AS
	DELETE FROM T_RMCompanyUsersOverall
	WHERE        (CompanyID = @companyID)
