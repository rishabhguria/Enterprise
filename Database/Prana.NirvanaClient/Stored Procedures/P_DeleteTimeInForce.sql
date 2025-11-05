


/****** Object:  Stored Procedure dbo.P_DeleteTimeInForce    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteTimeInForce
	(
		@timeInForceID int
	)
AS

--Delete Corresponding HandlingInstructions from the tables referring it.

		Declare @total int

		Select @total = Count(1) 
		FROM T_CVAUECTimeInForce	Where TimeInForceID = @timeInForceID
	
				if ( @total = 0)
					begin 		
						-- If TimeInForceID is not referenced anywhere.
						--Delete TimeInForce.
						Delete T_TimeInForce
						Where TimeInForceID = @timeInForceID
					end
					
			
	




