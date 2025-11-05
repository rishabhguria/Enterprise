


/****** Object:  Stored Procedure dbo.P_DeleteFix    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteFix
	(
		@fixID int
	)
AS

--Delete Corresponding Fix from the tables referring it.

		Declare @total int

		Select @total = Count(1) 
		FROM T_CVFIX	Where FixVersionID = @fixID
	
			if ( @total = 0)
			begin 		
				Select @total = Count(1) 
				FROM T_CompanyCompliance	Where FixVersionID = @fixID
				
				if( @total = 0)
				begin
					-- If FixID is not referenced anywhere.
					--Delete Fix.
					
					Delete T_Fix
					Where FixID = @fixID
				end
		end