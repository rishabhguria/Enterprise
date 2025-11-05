CREATE PROCEDURE dbo.P_DeleteRMDefaultCompanyAlerts
	
	(
	@companyID int 
	)
	
AS

	DELETE FROM T_RMDefaultAlerts
	WHERE        (CompanyID = @companyID)
