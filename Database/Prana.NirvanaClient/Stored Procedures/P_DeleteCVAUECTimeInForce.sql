


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECTimeInForce    Script Date: 12/28/2005 2:58:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECTimeInForce
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECTimeInForce
	Where CVAUECID = @cvAUECID



