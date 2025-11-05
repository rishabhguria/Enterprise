
/****** Object:  Stored Procedure dbo.P_DeleteCounterParty    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteCounterParty
	(
		@counterPartyID int
	)
AS

		Declare @total int
	Select @total = Count(1) 
		From T_CompanyCounterParties
		Where CounterPartyID = @counterPartyID
	
			if ( @total = 0)
			begin

				Select @total = Count(1) 
				From T_CounterPartyVenue
				Where CounterPartyID = @counterPartyID
			
					if ( @total = 0)
					begin 		
						-- If CounterParty is not referenced anywhere.
						--Delete CounterParty.
						

						Delete T_CounterParty
						Where CounterPartyID = @counterPartyID
						
					end
			end


