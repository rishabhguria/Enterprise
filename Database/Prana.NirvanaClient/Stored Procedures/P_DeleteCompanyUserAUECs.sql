


/****** Object:  Stored Procedure dbo.P_DeleteCompanyUserAUECs    Script Date: 01/11/2006 9:15:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyUserAUECs
	(
		@companyUserID int
	)
AS
	Delete T_CompanyUserAUEC
	Where CompanyUserID = @companyUserID



