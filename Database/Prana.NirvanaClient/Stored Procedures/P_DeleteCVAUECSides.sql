


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECSides    Script Date: 12/28/2005 2:50:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECSides
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECSide
	Where CVAUECID = @cvAUECID



