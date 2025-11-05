


/****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterParties    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCounterParties
	(
		@companyID int
	)
AS
	Delete T_CompanyCounterParties
	Where CompanyID = @companyID



/*ALTER PROCEDURE dbo.P_DeleteCompanyCounterParties
	(
		@companyID int
	)
AS
	Delete T_CompanyCounterPartyVenues
	Where CompanyID = @companyID */



