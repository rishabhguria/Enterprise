
/****** Object:  Stored Procedure dbo.P_GetCVCurrencies    Script Date: 12/21/2005 2:15:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVCurrencies
	(
		@counterPartyVenueID int		
	)
AS
	
	SELECT CounterPartyVenueID, CurrencyID
	FROM	T_CVCurrency
	Where  CounterPartyVenueID = @counterPartyVenueID