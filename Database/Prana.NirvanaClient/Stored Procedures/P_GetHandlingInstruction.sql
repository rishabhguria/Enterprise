


/****** Object:  Stored Procedure dbo.P_GetHandlingInstruction    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetHandlingInstruction
(
		@handlingInstructionsID int
)
AS
SELECT     HandlingInstructionsID, HandlingInstructions, HandlingInstructionsTagValue
FROM         T_HandlingInstructions
	Where HandlingInstructionsID = @handlingInstructionsID



