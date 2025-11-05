


/****** Object:  Stored Procedure dbo.P_DeleteFutureMonthCode    Script Date: 09/12/2005 11:55:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteFutureMonthCode
	(
		@futureMonthCodeID int,	
		@deleteForceFully int
	)
AS

--Delete Corresponding FutureMonthCode from the tables referring it.
	if ( @deleteForceFully = 1)
	begin 
		-- If Identifier is referenced anywhere and still we want to delete it.
		--Delete Identifier and related information.
		
		--Delete T_AUECComplaince
				--Where IdentifierID = @identifierID
		
		--Delete T_CompanyCounterPartyVenueDetails
				--Where CMTAGiveUp = @identifierID
							
				Delete T_FutureMonthCode
				Where FutureMonthCodeID = @futureMonthCodeID
	end
	
	else
	begin
		Declare @total int

		/*Select @total = Count(1) 
		From T_AUECCompliance AS C
			Where C.IdentifierID = @identifierID
			
		if ( @total = 0)
		begin
		
			Select @total = Count(1) 
			FROM         T_CompanyCounterPartyVenueDetails AS T
				Where T.CMTAGiveUp = @identifierID
		
				if ( @total = 0)
				begin 		*/
					-- If CurrencyTypeID is not referenced anywhere.
					--Delete CurrencyType.
					
												
					Delete T_FutureMonthCode
				Where FutureMonthCodeID = @futureMonthCodeID

		/*		end
		end	
		else
		begin
			return -1
		end */
	end
	




