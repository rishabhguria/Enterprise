


/****** Object:  Stored Procedure dbo.P_SaveCVAUECSide    Script Date: 12/28/2005 2:25:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECTimeInForce
	(
		@cvAUECID int,
		@timeInForceID int
	)
AS
		--Insert Data
		INSERT INTO T_CVAUECTimeInForce(CVAUECID, TimeInForceID)
			
		Values(@cvAUECID, @timeInForceID)


