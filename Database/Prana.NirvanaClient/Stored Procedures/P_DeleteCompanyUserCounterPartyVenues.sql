


/****** Object:  Stored Procedure dbo.P_DeleteCompanyUserCounterPartyVenues    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyUserCounterPartyVenues

	(
		@companyID int
	)

AS
	Delete T_CompanyUserCounterPartyVenues
	Where (CompanyUserID in (SELECT UserID 
							FROM T_CompanyUser WHERE 
							CompanyID = @companyID) )
	AND (CounterPartyVenueID not in ( SELECT CounterPartyVenueID
										FROM T_CompanyCounterPartyVenues
										WHERE CompanyID = @companyID))




