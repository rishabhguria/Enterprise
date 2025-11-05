

CREATE PROCEDURE dbo.P_DeleteRMCompanyAlerts
	(
		@companyID int
	)
AS
	Delete T_RMCompanyAlerts
	Where CompanyID = @companyID

