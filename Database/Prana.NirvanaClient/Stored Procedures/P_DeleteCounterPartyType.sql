


/****** Object:  Stored Procedure dbo.P_DeleteCounterPartyType    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteCounterPartyType
	(
		@counterPartyTypeID int
	)
AS

--Delete Corresponding CounterPartyType from the tables referring it.

		Declare @total int

		Select @total = Count(1) 
		From T_CounterParty AS CP
			Where CP.CounterPartyTypeID = @counterPartyTypeID
			
			
				if ( @total = 0)
				begin 		
					-- If CounterPartyTypeID is not referenced anywhere.
					--Delete CounterPartyType.
					
												
					Delete T_CounterPartyType
					Where CounterPartyTypeID = @CounterPartyTypeID

				end
		
	




