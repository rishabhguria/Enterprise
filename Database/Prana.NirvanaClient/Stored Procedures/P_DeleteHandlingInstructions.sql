


/****** Object:  Stored Procedure dbo.P_DeleteHandlingInstructions    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteHandlingInstructions
	(
		@handlingInstructionsID int
	)
AS

--Delete Corresponding HandlingInstructions from the tables referring it.

	Declare @total int

		Select @total = Count(1) 
		FROM T_CVAUECHandlingInstructions	Where HandlingInstructionsID = @handlingInstructionsID
	
				if ( @total = 0)
					begin 		
						-- If HandlingInstructions is not referenced anywhere.
						--Delete HandlingInstructions.
				
						Delete T_HandlingInstructions
					Where HandlingInstructionsID = @handlingInstructionsID
				end
	



	




