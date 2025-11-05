


/****** Object:  Stored Procedure dbo.P_GetAllHandlingInstructions    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllHandlingInstructions
	
AS
	SELECT     HandlingInstructionsID, HandlingInstructions, HandlingInstructionsTagValue
FROM         T_HandlingInstructions Order By HandlingInstructions


