

/****** Object:  Stored Procedure dbo.P_DeleteCompanyBorrower    Script Date: 03/01/2005 1:14:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyBorrower
	(
		@companyID int
	)
AS
	Delete T_CompanyBorrower
	Where CompanyID = @companyID

