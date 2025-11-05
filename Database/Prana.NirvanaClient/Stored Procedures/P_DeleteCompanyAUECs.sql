


/****** Object:  Stored Procedure dbo.P_DeleteCompanyAUECs    Script Date: 01/05/2005 3:19:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyAUECs
	(
		@companyID int
	)
AS
	Delete T_CompanyAUEC
	Where CompanyID = @companyID



