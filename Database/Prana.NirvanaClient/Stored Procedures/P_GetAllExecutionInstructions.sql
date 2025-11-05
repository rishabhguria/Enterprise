


/****** Object:  Stored Procedure dbo.P_GetAllExecutionInstructions    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllExecutionInstructions

AS
	SELECT     ExecutionInstructionsID, ExecutionInstructions, ExecutionInstructionsTagValue
FROM         T_ExecutionInstructions Order By ExecutionInstructions


