


/****** Object:  Stored Procedure dbo.P_DeleteCompanyCompliance    Script Date: 03/01/2005 1:11:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCompliance
	(
		@companyID int
	)
AS
	Delete T_CompanyCompliance
	Where CompanyID = @companyID



