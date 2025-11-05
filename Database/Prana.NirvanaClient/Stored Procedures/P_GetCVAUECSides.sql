


/****** Object:  Stored Procedure dbo.P_GetCVAUECSides    Script Date: 12/28/2005 4:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECSides
	(
		@cvAuecID int		
	)
AS
	
	SELECT CVAUECID, SideID
	FROM	T_CVAUECSide
	Where  CVAUECID = @cvAuecID

