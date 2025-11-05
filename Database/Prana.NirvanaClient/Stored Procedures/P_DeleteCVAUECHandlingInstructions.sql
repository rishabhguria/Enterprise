


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECHandlingInstructions    Script Date: 12/28/2005 3:00:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECHandlingInstructions
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECHandlingInstructions
	Where CVAUECID = @cvAUECID



