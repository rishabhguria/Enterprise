


/****** Object:  Stored Procedure dbo.P_DeleteSide    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteSide
	(
		@sideID int
	)
AS

--Delete Corresponding Side from the tables referring it.
	Declare @total int

		Select @total = Count(1) 
		FROM T_AUECSide	Where SideID = @sideID
	
				if ( @total = 0)
				begin
				
					Select @total = Count(1) 
					FROM T_CVAUECSide	Where SideID = @sideID
	
					if ( @total = 0)
					begin 		
						-- If SideID is not referenced anywhere.
						--Delete SideID.
				
						Delete T_Side
						Where SideID = @sideID
					end
				end


