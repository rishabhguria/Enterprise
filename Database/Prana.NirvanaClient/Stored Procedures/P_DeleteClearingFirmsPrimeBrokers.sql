


/****** Object:  Stored Procedure dbo.P_DeleteClearingFirmsPrimeBrokers    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteClearingFirmsPrimeBrokers
	(
		@clearingFirmsPrimeBrokersID int,	
		@deleteForceFully int
	)
AS

--Delete Corresponding ClearingFirmsPrimeBrokers from the tables referring it.
	if ( @deleteForceFully = 1)
	begin 
		-- If ClearingFirmsPrimeBrokers is referenced anywhere and still we want to delete it.
		--Delete ClearingFirmsPrimeBrokers and related information.
		
		--Delete T_CompanyClearingFirmsPrimeBrokers
				--Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID 
				
				--** The above query to be rewritten as the database table
				--**is to be changed. So have to modify it again.
				
		
									
				Delete T_ClearingFirmsPrimeBrokers
				Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID
	end
	
	else
	begin
		Declare @total int

		--Select @total = Count(1) 
		--From T_CompanyClearingFirmsPrimeBrokers AS SM
		--	Where SM.ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID
		
		--** The above query to be rewritten as the database table
		--**is to be changed. So have to modify it again.
			
			
		--		if ( @total = 0)
		--		begin 		
					-- If SymbolIdentifier is not referenced anywhere.
					--Delete SymbolIdentifier.
					
												
					Delete T_ClearingFirmsPrimeBrokers
					Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID

		--		end
			--else
		--begin
		--	return -1
		--end
	end
	




