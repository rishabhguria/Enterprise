


/****** Object:  Stored Procedure dbo.P_InsertAUECSide    Script Date: 12/20/2005 4:40:24 PM ******/
CREATE PROCEDURE dbo.P_InsertAUECSide
	(
		@auecID int,
		@SideID int
		
	)
AS
		--Insert Data
		INSERT INTO T_AUECSide(AUECID, SideID)
			
		Values(@auecID, @SideID)



