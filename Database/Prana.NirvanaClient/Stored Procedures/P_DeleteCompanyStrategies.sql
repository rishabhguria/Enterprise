


/****** Object:  Stored Procedure dbo.P_DeleteCompanyStrategies    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyStrategies
	(
		@companyID int
	)
AS
	Delete T_CompanyStrategy
	Where CompanyID = @companyID


