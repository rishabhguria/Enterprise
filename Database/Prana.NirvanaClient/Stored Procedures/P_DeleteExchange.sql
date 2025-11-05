
/****** Object:  Stored Procedure dbo.P_DeleteExchange    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteExchange
	(   
		@exchangeID int
	)
	
AS
	--Delete Corresponding Exchanges	
	Declare @total int

	Select @total = Count(1) 
	From T_AUEC
	Where ExchangeID = @exchangeID

	if ( @total = 0)
	begin 		
		-- If Exchange is not referenced anywhere.
		DELETE T_ExchangeHolidays
		Where ExchangeID = @exchangeID
		
		DELETE T_Exchange
		Where ExchangeID = @exchangeID

	end	
