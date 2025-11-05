
/****** Object:  Stored Procedure dbo.P_DeleteUnit    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteUnit
	(
		@unitID int
	)
AS

--Delete Corresponding Unit from the tables referring it.
	
		Declare @total int

		Select @total = Count(1) 
		From T_AUEC Where UnitID = @unitID
			
		if ( @total = 0)
		begin
		
					
			-- If UnitID is not referenced anywhere.
			--Delete Unit.
			
			Delete T_Units
			Where UnitID = @unitID

		end
	


