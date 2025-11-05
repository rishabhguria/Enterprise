


/****** Object:  Stored Procedure dbo.P_SaveCVAUECSide    Script Date: 12/28/2005 2:15:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECSide
	(
		@cvAUECID int,
		@SideID int
	)
AS
		--Insert Data
		INSERT INTO T_CVAUECSide(CVAUECID, SideID)
			
		Values(@cvAUECID, @SideID)



