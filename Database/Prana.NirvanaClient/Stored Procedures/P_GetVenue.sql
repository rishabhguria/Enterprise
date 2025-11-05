
/****** Object:  Stored Procedure dbo.P_GetVenue    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetVenue
	(
		@venueID int
	)
AS
	SELECT     VenueID, VenueName, VenueTypeID, Route, ExchangeID
	FROM         T_Venue
	Where VenueID = @venueID

