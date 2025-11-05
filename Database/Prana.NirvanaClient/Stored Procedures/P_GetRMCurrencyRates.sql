CREATE PROCEDURE dbo.P_GetRMCurrencyRates

AS

	SELECT     FromCurrencyID, ToCurrencyID, ConversionType, ConversionFactor
	FROM         T_RMCurrencyConversion
 
