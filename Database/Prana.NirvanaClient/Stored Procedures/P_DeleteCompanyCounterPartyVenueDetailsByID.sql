


/****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterPartyVenueDetailsByID    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCounterPartyVenueDetailsByID
(
		@companyCounterPartyVenueID int	
)
AS
Delete T_CompanyCounterPartyVenueDetails
Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID



