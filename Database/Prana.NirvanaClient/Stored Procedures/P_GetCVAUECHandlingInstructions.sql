


/****** Object:  Stored Procedure dbo.P_GetCVAUECHandlingInstructions    Script Date: 12/28/2005 4:33:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECHandlingInstructions
	(
		@cvAuecID int		
	)
AS
	
	SELECT CVAUECID, HandlingInstructionsID
	FROM	T_CVAUECHandlingInstructions	
	Where  CVAUECID = @cvAuecID

