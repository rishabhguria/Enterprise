


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientFunds    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientFunds
(
	@companyClientID int
)
AS
	
	Delete T_CompanyClientFund
	Where CompanyClientID = @companyClientID


