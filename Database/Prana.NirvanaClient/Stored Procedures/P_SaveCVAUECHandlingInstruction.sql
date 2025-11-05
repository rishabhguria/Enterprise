


/****** Object:  Stored Procedure dbo.P_SaveCVAUECHandlingInstruction    Script Date: 12/28/2005 2:30:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECHandlingInstruction
	(
		@cvAUECID int,
		@handlingInstructionID int
	)
AS
		--Insert Data
		INSERT INTO T_CVAUECHandlingInstructions(CVAUECID, HandlingInstructionsID)
			
		Values(@cvAUECID, @handlingInstructionID)


