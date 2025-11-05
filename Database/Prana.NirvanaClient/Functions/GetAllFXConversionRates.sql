-- =============================================      
-- Author:  Sumit Kakra      
-- Create date: 19-MARCH-2008      
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs      
-- Usage:       
-- Declare @Date datetime      
-- Set @Date = DateAdd(d,-3,GetUTCDate())      
-- Select * from GetAllFXConversionRates('2008-03-25') --@Date)    
-- TODO : For now we have picked up all the conversion rates. We need to pickup only those conversion rates which are for open positions/unallocated trades.
-- =============================================       
CREATE FUNCTION [dbo].[GetAllFXConversionRates] () 
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
    
  UNION    
    
  Select StanPair.ToCurrencyID AS FromCurrencyID,      
      StanPair.FromCurrencyID AS ToCurrencyID,      
      CCR.ConversionRate AS RateValue,       
      1 AS ConversionMethod,       
      CCR.Date AS Date,      
      StanPair.eSignalSymbol AS eSignalSymbol      
   from T_CurrencyConversionRate AS CCR      
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK   
  
    
 RETURN    
END
