


/****** Object:  Stored Procedure dbo.P_DeleteVenueType    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteVenueType
	(
		@venueTypeID int
	)
AS

--Delete Corresponding VenueType from the tables referring it.

		Declare @total int

		Select @total = Count(1) 
		From T_Venue AS V
			Where V.VenueTypeID = @venueTypeID
			
			
				if ( @total = 0)
				begin 		
					-- If VenueTypeID is not referenced anywhere.
					--Delete VenuType.
					
												
					Delete T_VenuType
					Where VenueTypeID = @venueTypeID

				end
		
	




