


/****** Object:  Stored Procedure dbo.P_DeleteSetUpCompany    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteSetUpCompany
	(
		@companyID int
	)
AS
	Delete T_Company
	Where CompanyID = @companyID



