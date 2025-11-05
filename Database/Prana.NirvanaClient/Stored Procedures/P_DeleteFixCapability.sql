


/****** Object:  Stored Procedure dbo.P_DeleteFixCapability    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteFixCapability
	(
		@fixCapabilityID int
	)
AS

--Delete Corresponding FixCapability from the tables referring it.
	
		Declare @total int

		Select @total = Count(1) 
		FROM         T_CompanyCompliance Where FixCapabilityID = @fixCapabilityID
	
			if ( @total = 0)
			begin 		
				-- If FixCapabilityID is not referenced anywhere.
				--Delete FixCapability.
				
				
							
				Delete T_FixCapability
				Where FixCapabilityID = @fixCapabilityID

		end
		
		
	




