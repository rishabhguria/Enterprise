


/****** Object:  Stored Procedure dbo.P_DeleteAUECSides    Script Date: 12/21/2005 4:40:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteAUECSides
	(
		@auecID int
	)
AS
	Delete T_AUECSide
	Where AUECID = @auecID



