


/****** Object:  Stored Procedure dbo.P_DeleteCVCurrencies    Script Date: 12/23/2005 11:30:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCVCurrencies
	(
		@counterPartyVenueID int
	)
AS
	Delete T_CVCurrency
	Where CounterPartyVenueID = @counterPartyVenueID



