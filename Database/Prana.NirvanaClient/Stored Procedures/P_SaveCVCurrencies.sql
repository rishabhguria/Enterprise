
/****** Object:  Stored Procedure dbo.P_SaveCVCurrencies    Script Date: 12/23/2005 11:32:24 AM ******/
CREATE PROCEDURE dbo.P_SaveCVCurrencies
	(
		@counterPartyVenueID int,
		@currencyID int,
		@result int
	)
AS
		--Insert Data
		INSERT INTO T_CVCurrency(CounterPartyVenueID, CurrencyID)
			
		Values(@counterPartyVenueID, @currencyID)
		
			Set @result = scope_identity()
		
select @result


