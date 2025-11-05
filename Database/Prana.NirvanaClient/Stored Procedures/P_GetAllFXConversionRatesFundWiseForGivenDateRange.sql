  
/********************************************************                           
 Author:  Narendra Kumar jangir                            
 Create date: Mar 11,2015                            
 Description: Returns fund wise Forex rate with standard pairs  and reverse of standard pairs for the date range given 
 This SP is referred by P_GetAllFXConversionRatesForGivenDateRange, only difference is that fundid added to fetch fund wise fx rate.             
 Usage:                             
 Exec [P_GetAllFXConversionRatesFundWiseForGivenDateRange] '11-01-2015','03-20-2015' 

********************************************************/                             
CREATE Procedure [P_GetAllFXConversionRatesFundWiseForGivenDateRange]           
(          
@startingDate datetime,           
@endingDate datetime          
)          
AS                            
BEGIN                            
 -- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.                          
                         
  Select StanPair.FromCurrencyID AS FromCurrencyID,                            
      StanPair.ToCurrencyID AS ToCurrencyID,                            
      Isnull(CCR.ConversionRate,0) AS RateValue,                             
      0 AS ConversionMethod,                             
      CCR.Date AS Date,                            
      StanPair.eSignalSymbol AS eSignalSymbol,
	  CCR.FundID AS FundID                           
   From T_CurrencyConversionRate AS CCR                            
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                       
   WHERE DateDiff(d,@startingDate,CCR.Date) >=0 -- dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)                
  AND DateDiff(d,CCR.Date,@endingDate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                  
                          
  UNION                          
                          
  Select StanPair.ToCurrencyID AS FromCurrencyID,                            
      StanPair.FromCurrencyID AS ToCurrencyID,                            
      Isnull(CCR.ConversionRate,0) AS RateValue,                             
      1 AS ConversionMethod,                             
      CCR.Date AS Date,                            
      StanPair.eSignalSymbol AS eSignalSymbol,
	  CCR.FundID AS FundID                             
   from T_CurrencyConversionRate AS CCR                            
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                         
   WHERE DateDiff(d,@startingDate,CCR.Date) >=0 --dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)                
  AND DateDiff(d,CCR.Date,@endingDate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                     
                          
                       
END 
