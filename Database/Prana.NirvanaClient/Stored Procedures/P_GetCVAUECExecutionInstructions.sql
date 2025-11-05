


/****** Object:  Stored Procedure dbo.P_GetCVAUECExecutionInstructions    Script Date: 12/28/2005 4:35:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECExecutionInstructions
	(
		@cvAuecID int		
	)
AS
	
	SELECT CVAUECID, ExecutionInstructionsID
	FROM	T_CVAUECExecutionInstructions	
	Where  CVAUECID = @cvAuecID

