---Author : Rajat  
---Date : 27 Sep 2006
---P_GetCurrencyCodeConversion is very much same as P_GetCurrencyConversion but the difference is that we are using
---the CurrencySymbol rather than CurrencyName from T_Currency table
CREATE PROCEDURE [dbo].[P_GetCurrencyCodeConversion]  
AS  
SELECT C1.CURRENCYID AS FromCurrencyID, C1.CurrencySymbol as FromCurrencyName, C2.CURRENCYID AS ToCurrencyID,   
C2.CurrencySymbol as ToCurrencyName, CC.ConversionFactor as ConversionFactor, CC.ConversionType as ConversionType from T_CURRENCYCONVERSION CC  
JOIN T_CURRENCY C1 ON C1.CURRENCYID = CC.FROMCURRENCYID  
JOIN T_CURRENCY C2 ON C2.CURRENCYID = CC.TOCURRENCYID 