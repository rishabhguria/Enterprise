


/****** Object:  Stored Procedure dbo.P_DeleteExecutionInstruction    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteExecutionInstruction
	(
		@executionInstructionsID int
	)
AS

--Delete Corresponding ExecutionInstruction from the tables referring it.
	Declare @total int

		Select @total = Count(1) 
		FROM T_CVAUECExecutionInstructions	Where ExecutionInstructionsID = @executionInstructionsID
	
				if ( @total = 0)
					begin 		
						-- If ExecutionInstruction is not referenced anywhere.
						--Delete ExecutionInstruction.
				
						Delete T_ExecutionInstructions
						Where ExecutionInstructionsID = @executionInstructionsID
				end
	




