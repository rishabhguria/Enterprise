


/****** Object:  Stored Procedure dbo.P_DeleteModule    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteModule
	(
		@moduleID int
	)
AS

--Delete Corresponding Module from the tables referring it.
		Declare @total int

		Select @total = Count(1) 
		From T_CompanyModule AS CM
			Where CM.ModuleID = @moduleID
			
		if ( @total = 0)
		begin
		
			-- If ModuleID is not referenced anywhere.
			--Delete Module.
					
												
			Delete T_Module
			Where ModuleID = @moduleID

		end	
	




