-- =============================================              
-- Author:  Bhupesh Bareja              
-- Create date: 12-JUN-2008              
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs for the given date only             
-- Usage:               
-- Select * from GetAllFXConversionRatesForGivenDate('06-12-2008')            
-- =============================================               
CREATE FUNCTION [dbo].[GetAllFXConversionRatesForGivenDate] (@date datetime)         
RETURNS @FXConversionRatesForDate TABLE             
 (            
  FromCurrencyID int,            
  ToCurrencyID int,            
  RateValue float,            
  ConversionMethod int,            
  Date DateTime,            
  eSignalSymbol varchar(max)            
 )             
AS              
BEGIN              
 -- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.            
            
 INSERT INTO @FXConversionRatesForDate            
  Select StanPair.FromCurrencyID AS FromCurrencyID,              
      StanPair.ToCurrencyID AS ToCurrencyID,              
      CCR.ConversionRate AS RateValue,               
      0 AS ConversionMethod,               
      CCR.Date AS Date,              
      StanPair.eSignalSymbol AS eSignalSymbol              
   From T_CurrencyConversionRate AS CCR              
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK         
   WHERE datediff(d,CCR.Date,@date) = 0
            
  UNION            
            
  Select StanPair.ToCurrencyID AS FromCurrencyID,              
      StanPair.FromCurrencyID AS ToCurrencyID,              
      CCR.ConversionRate AS RateValue,               
      1 AS ConversionMethod,               
      CCR.Date AS Date,              
      StanPair.eSignalSymbol AS eSignalSymbol              
   from T_CurrencyConversionRate AS CCR              
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK           
   WHERE datediff(d,CCR.Date,@date) = 0
            
 RETURN            
END 
