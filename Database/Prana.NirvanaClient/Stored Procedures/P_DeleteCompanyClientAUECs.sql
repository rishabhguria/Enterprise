


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientAUECs    Script Date: 01/24/2006 4:40:24 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientAUECs
(
	@companyClientID int
)
AS
	
	Delete T_CompanyClientAUEC
	Where CompanyClientID = @companyClientID


