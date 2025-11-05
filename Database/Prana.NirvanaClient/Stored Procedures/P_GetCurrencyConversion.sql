CREATE PROCEDURE [dbo].[P_GetCurrencyConversion]
AS
SELECT 
	C1.CURRENCYID AS FromCurrencyID, 
	C1.CurrencyName as FromCurrencyName, 
	C2.CURRENCYID AS ToCurrencyID, 
	C2.CurrencyName as ToCurrencyName, 
	CC.ConversionFactor as ConversionFactor, 
	CC.ConversionType as ConversionType 
FROM 
	T_CURRENCYCONVERSION CC
JOIN 
	T_CURRENCY C1 ON C1.CURRENCYID = CC.FROMCURRENCYID
JOIN 
	T_CURRENCY C2 ON C2.CURRENCYID = CC.TOCURRENCYID