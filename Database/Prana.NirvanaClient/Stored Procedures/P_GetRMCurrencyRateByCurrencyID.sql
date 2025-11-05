CREATE PROCEDURE dbo.P_GetRMCurrencyRateByCurrencyID
(
	@currencyID int

)

AS

	SELECT     FromCurrencyID, ToCurrencyID, ConversionType, ConversionFactor
	FROM         T_RMCurrencyConversion
	WHERE     (FromCurrencyID = @currencyID)
 
