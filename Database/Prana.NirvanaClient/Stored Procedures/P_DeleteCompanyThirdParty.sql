


/****** Object:  Stored Procedure dbo.P_DeleteCompanyThirdParty    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyThirdParty
	(
		@companyID int
	)
AS
	Delete T_CompanyThirdParty
	Where CompanyID = @companyID



