CREATE PROCEDURE dbo.P_GetCurrencyConverion
	(
		@fromCurrencyID int,
		@toCurrencyID int
	)
As
SELECT        FromCurrencyID, ToCurrencyID, ConversionType, ConversionFactor
FROM            T_RMCurrencyConversion
WHERE        (FromCurrencyID = @fromCurrencyID) AND (ToCurrencyID = @toCurrencyID)
	
