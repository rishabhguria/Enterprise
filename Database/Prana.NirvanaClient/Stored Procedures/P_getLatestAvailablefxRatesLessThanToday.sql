  
  
CREATE  PROCEDURE [dbo].[P_getLatestAvailablefxRatesLessThanToday]   
  
As  
  
SELECT CurrencyPairID_Fk AS CPI, Max(Date) AS MaxDate, tocurrencyId 
INTO #temp2  
FROM T_currencyConversionRate WITH (NOLOCK) 
INNER JOIN T_CurrencyStandardPairs WITH (NOLOCK) 
ON CurrencyPairID_Fk = CurrencyPairID  
GROUP BY CurrencyPairID_Fk, tocurrencyId  
  
SELECT CurrencyPairID_Fk, ConversionRate, Date 
INTO #temp1  
FROM T_currencyConversionRate WITH (NOLOCK) 
  
SELECT #temp2.tocurrencyId AS ToCurrencyId, #temp2.MaxDate , #temp1.ConversionRate   
FROM #temp2  
inner join #temp1  
ON #temp2.CPI = #temp1.CurrencyPairID_Fk  
WHERE #temp2.MaxDate = #temp1.Date  
ORDER BY MaxDate DESC  
  
DROP TABLE #temp1  
DROP TABLE #temp2  
  
  
  
  