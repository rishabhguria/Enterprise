


/****** Object:  Stored Procedure dbo.P_DeleteVenue    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteVenue
	(
		@venueID int
	)
AS
		Declare @total int

		Select @total = Count(1) 
		From T_CounterPartyVenue
		Where VenueID = @venueID
	
		if ( @total = 0)
		begin 
			--Delete Venue.
			
			Delete T_Venue
			Where VenueID = @venueID
		
		end





