


/****** Object:  Stored Procedure dbo.P_GetExecutionInstruction    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetExecutionInstruction
(
		@executionInstructionsID int
)
AS
	SELECT     ExecutionInstructionsID, ExecutionInstructions, ExecutionInstructionsTagValue
FROM         T_ExecutionInstructions
Where ExecutionInstructionsID = @executionInstructionsID
 



