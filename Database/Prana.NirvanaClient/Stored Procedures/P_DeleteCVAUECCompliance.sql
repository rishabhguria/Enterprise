


/****** Object:  Stored Procedure dbo.P_DeleteCVAUECCompliance    Script Date: 01/03/2005 5:30:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVAUECCompliance
	(
		@cvAUECID int
	)
AS
	Delete T_CVAUECCompliance
	Where CVAUECID = @cvAUECID



