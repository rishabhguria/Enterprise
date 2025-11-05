


/****** Object:  Stored Procedure dbo.P_SaveCVAUECExecutionInstruction    Script Date: 12/28/2005 2:35:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECExecutionInstruction
	(
		@cvAUECID int,
		@executionInstructionID int
	)
AS
		--Insert Data
		INSERT INTO T_CVAUECExecutionInstructions(CVAUECID, ExecutionInstructionsID)
			
		Values(@cvAUECID, @executionInstructionID)


