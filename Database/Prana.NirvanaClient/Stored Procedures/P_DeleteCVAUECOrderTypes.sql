


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECOrderTypes    Script Date: 12/28/2005 2:55:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECOrderTypes
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECOrderTypes
	Where CVAUECID = @cvAUECID



