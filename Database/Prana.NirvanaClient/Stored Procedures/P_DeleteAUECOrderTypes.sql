


/****** Object:  Stored Procedure dbo.P_DeleteAUECOrderTypes    Script Date: 12/21/2005 4:45:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteAUECOrderTypes
	(
		@auecID int
	)
AS
	Delete T_AUECOrderTypes
	Where AUECID = @auecID



