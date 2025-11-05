


/****** Object:  Stored Procedure dbo.P_GetCurrency    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCurrency
	(
		@currencyID int
	)
AS
	SELECT CurrencyID, CurrencyName, CurrencySymbol
	FROM T_Currency
	Where CurrencyID = @currencyID


