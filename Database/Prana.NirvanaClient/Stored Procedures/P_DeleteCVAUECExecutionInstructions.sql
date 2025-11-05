


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECExecutionInstructions    Script Date: 12/28/2005 3:02:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECExecutionInstructions
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECExecutionInstructions
	Where CVAUECID = @cvAUECID



