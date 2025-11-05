


/****** Object:  Stored Procedure dbo.P_DeleteCompanyUserCounterParties    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyUserCounterParties
	(
		@companyUserID int
	)
AS
	Delete T_CompanyUserCounterPartyVenues
	Where CompanyUserID = @companyUserID


