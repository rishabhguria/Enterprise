


/****** Object:  Stored Procedure dbo.P_GetCVAUECTimeInForces    Script Date: 12/28/2005 4:30:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECTimeInForces
	(
		@cvAuecID int		
	)
AS
	
	SELECT CVAUECID, TimeInForceID
	FROM	T_CVAUECTimeInForce	
	Where  CVAUECID = @cvAuecID

