-- =============================================            
-- Author:  Sandeep           
-- Create date: 23-Oct-2008            
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs            
-- Usage:             
-- Declare @Date datetime            
-- Set @Date = DateAdd(d,-3,GetUTCDate())            
-- Select * from GetFXConversionRatesForDateRanges('2008-05-05','2008-10-23') order by Date --@Date)          
-- =============================================             
CREATE FUNCTION [dbo].[GetFXConversionRatesForDateRanges]   
(          
 @StartDate datetime,  
 @EndDate DateTime--,    
 --@isHistoricalModeAllowed bit     -- if 1 is passed then the function will return historical data otherwise on for the given date.    
)            
RETURNS @FXConversionRatesForDate TABLE           
 (          
  FromCurrencyID int,          
  ToCurrencyID int,          
  RateValue float,          
  ConversionMethod int,          
  Date DateTime,          
  eSignalSymbol varchar(max),      
  InputDatePriceIndicator int -- return 0 if supplied date's fx price is available, if date is less than the supplied date, then return 1                
 )           
AS            
BEGIN            
 Declare @tblMaxDateForCurrencyPair Table          
  (CurrencyPairId int,           
   MaxDate datetime)          
          
--if (@isHistoricalModeAllowed = 1)     
begin     
 ---------returns historical data.------------    
 Insert into @tblMaxDateForCurrencyPair          
   Select CurrencyPairID_FK ,Date           
    From T_CurrencyConversionRate InnerCCR            
    Where Date >= @StartDate and Date <= @EndDate and (InnerCCR.ConversionRate <> null or InnerCCR.ConversionRate <> 0)  --@Date  -- Check if we need to apply GetFormattedDatePart function here.          
--    Group By CurrencyPairID_FK            
           
  -- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.          
end    
--else    
--begin    
-- ---------returns data on the data passed.------------    
-- Insert into @tblMaxDateForCurrencyPair          
--   Select CurrencyPairID_FK ,MAX(Date)           
--    From T_CurrencyConversionRate InnerCCR            
--    Where dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@StartDate) --and (InnerCCR.ConversionRate <> null or InnerCCR.ConversionRate <> 0)  --@Date  -- Check if we need to apply GetFormattedDatePart function here.          
--    Group By CurrencyPairID_FK            
--           
--  -- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.          
--end    
          
 INSERT INTO @FXConversionRatesForDate          
  Select StanPair.FromCurrencyID AS FromCurrencyID,            
      StanPair.ToCurrencyID AS ToCurrencyID,            
      CCR.ConversionRate AS RateValue,             
      0 AS ConversionMethod,             
      CCR.Date AS Date,            
      StanPair.eSignalSymbol AS eSignalSymbol,      
   Case       
  when datediff(d,CCR.Date,@StartDate) = 0 then 0      
  else 1       
   End as InputDatePriceIndicator      
   From T_CurrencyConversionRate AS CCR            
   INNER JOIN @tblMaxDateForCurrencyPair MaxCurrPair on CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairId And CCR.Date = MaxCurrPair.MaxDate          
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId          
          
  UNION All          
          
  Select StanPair.ToCurrencyID AS FromCurrencyID,            
      StanPair.FromCurrencyID AS ToCurrencyID,            
      CCR.ConversionRate AS RateValue,             
      1 AS ConversionMethod,             
      CCR.Date AS Date,            
      StanPair.eSignalSymbol AS eSignalSymbol,      
   Case       
  when datediff(d,CCR.Date,@StartDate) = 0 then 0      
  else 1       
   End as InputDatePriceIndicator      
   from T_CurrencyConversionRate AS CCR            
   INNER JOIN @tblMaxDateForCurrencyPair MaxCurrPair on CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairId And CCR.Date = MaxCurrPair.MaxDate          
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId          
          
  
 RETURN          
END 
