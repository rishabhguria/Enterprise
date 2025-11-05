


/****** Object:  Stored Procedure dbo.P_DeleteCompanyFunds    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyFunds
	(
		@companyID int
	)
AS
	Delete T_CompanyFunds
	Where CompanyID = @companyID


