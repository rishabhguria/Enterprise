


/****** Object:  Stored Procedure dbo.P_GetAUECSides    Script Date: 12/21/2005 2:15:22 PM ******/
CREATE PROCEDURE dbo.P_GetAUECSides
	(
		@auecID int		
	)
AS
	
	SELECT AUECID, SideID
	FROM	T_AUECSide
	Where  AUECID = @auecID

