
/****** Object:  Stored Procedure dbo.P_GetAllVenues    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllVenues
AS
	SELECT   VenueID, VenueName, VenueTypeID, Route, ExchangeID
FROM         T_Venue

