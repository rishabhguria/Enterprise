


/****** Object:  Stored Procedure dbo.P_DeleteUnderLying    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteUnderLying
	(
		@underlyingID int
	)

AS
		Declare @total int

		Select @total = Count(1) 
		From T_AUEC
		Where UnderLyingID = @underlyingID	
					
			if ( @total = 0)
			begin 		
				-- If UnderLying is not referenced anywhere.
	

				Delete T_Underlying 
				Where UnderLyingID = @underlyingID
			end	


