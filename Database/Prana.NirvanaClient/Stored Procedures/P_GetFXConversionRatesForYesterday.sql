 /*   
-- =============================================                    
      
-- Author:  Rajat tandon                  
      
-- Create date: 07-Sep-2009          
      
-- Description: Returns the Forex rate with only corresponding to the company base currency and reverse of it for yesterday                    
      
-- Usage:    exec P_GetFXConversionRatesForYesterday @YesterdayAUECDatesString=N'0^8/24/2012 6:16:42 AM~12^8/23/2012 2:16:41 AM~16^8/23/2012 2:16:41 AM~19^8/23/2012 2:16:41 AM~'                 
        
Modified By: Sandeep Singh, 
Date: August 24,2012, 
Desc: T_AUEC, T_Group tables replaced with temp tables #T_AUEC and #T_Group respectively 
*/   
      
---=============================================                     
      
CREATE PROCEDURE [dbo].[P_GetFXConversionRatesForYesterday]       
      
(                  
      
 @YesterdayAUECDatesString varchar(max)         
      
)                    
                 
               
      
AS                    
      
BEGIN           
        
--Keep Data in the Temp Tables
Select *
Into #T_Group from T_Group

Select *
Into #T_AUEC from T_AUEC     
      
-- Declare @YesterdayAUECdatesTable Table(AUECID int,LatestAUECDate DateTime)                                                   
--      
-- INSERT INTO @YesterdayAUECdatesTable Select * From dbo.GetAllAUECDatesFromString(@YesterdayAUECDatesString)           
Select AUECID, CurrentAUECDate as LatestAUECDate into #YesterdayAUECdatesTable From dbo.GetAllAUECDatesFromString(@YesterdayAUECDatesString)        
       
      
 declare @companyBaseCurrencyID int        
      
 Select @companyBaseCurrencyID = BaseCurrencyID from T_Company        
        
       
      
 Select distinct Min(auec.AUECID) as AUECID, InnerCCR.CurrencyPairID_FK as CurrencyPairID,Max(InnerCCR.Date) as MaxDate         
      
    into #TempCurrencyPairDates from #YesterdayAUECdatesTable yesterdayDates         
      
 inner join #T_AUEC auec on yesterdayDates.AUECID = auec.AUECID        
      
    inner join #T_Group G on G.AUECID = auec.AUECID      
      
      inner join T_CurrencyStandardPairs StanPairs on ((StanPairs.FromCurrencyID = G.CurrencyID and StanPairs.ToCurrencyID = @companyBaseCurrencyID)      
      
                                                                              or (StanPairs.FromCurrencyID = @companyBaseCurrencyID and StanPairs.ToCurrencyID = G.CurrencyID))      
      
 inner join T_CurrencyConversionRate InnerCCR on InnerCCR.CurrencyPairID_FK = StanPairs.CurrencyPairID         
      
 Where InnerCCR.Date <= yesterdayDates.LatestAUECDate and (InnerCCR.ConversionRate <> null or InnerCCR.ConversionRate <> 0)          
      
 Group By InnerCCR.CurrencyPairID_FK          
        
       
      
-- SELECT * FROM #TempCurrencyPairDates        
        
       
      
-- Select distinct Temp.AUECID, StanPairs.FromCurrencyID, StanPairs.ToCurrencyID, CCR.ConversionRate, Temp.MaxDate from #TempCurrencyPairDates Temp         
      
-- inner join T_CurrencyStandardPairs StanPairs on Temp.CurrencyPairID = StanPairs.CurrencyPairID        
      
-- inner join T_CurrencyConversionRate CCR on CCR.CurrencyPairID_FK = Temp.CurrencyPairID and DateDiff(d,Temp.MaxDate,CCR.Date) = 0        
        
       
      
 Create TABLE #FXConversionRatesForDate                    
      
 (                  
      
  FromCurrencyID int,                  
      
  ToCurrencyID int,                  
      
  RateValue float,                  
      
  ConversionMethod int,                  
      
  Date DateTime,                  
      
  eSignalSymbol varchar(max),              
      
  InputDatePriceIndicator int,          
      
  TickerSymbol varchar(50)    -- return 0 if supplied date's fx price is available, if date is less than the supplied date, then return 1                        
      
 )                   
         
            
      
 INSERT INTO #FXConversionRatesForDate                  
      
  Select StanPair.FromCurrencyID AS FromCurrencyID,                    
      
    StanPair.ToCurrencyID AS ToCurrencyID,                    
      
    CCR.ConversionRate AS RateValue,                     
      
    0 AS ConversionMethod,                     
      
    CCR.Date AS Date,                    
      
    StanPair.eSignalSymbol AS eSignalSymbol,              
      
  Case            
      
  when datediff(d,CCR.Date,yesterdayDates.LatestAUECDate) = 0 then 0              
      
  else 1               
      
  End as InputDatePriceIndicator ,            
      
  Currency1.CurrencySymbol +'-'+ Currency2.CurrencySymbol as TickerSymbol          
      
  From T_CurrencyConversionRate AS CCR                    
      
  INNER JOIN #TempCurrencyPairDates MaxCurrPair on CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairID And CCR.Date = MaxCurrPair.MaxDate                  
      
  INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId               
      
  INNER JOIN #YesterdayAUECdatesTable yesterdayDates on MaxCurrPair.AUECID = yesterdayDates.AUECID        
      
  INNER JOIN T_Currency as Currency1 on Currency1.CurrencyID = StanPair.FromCurrencyID          
      
  INNER JOIN T_Currency as Currency2 on Currency2.CurrencyID = StanPair.ToCurrencyID          
      
  WHERE StanPair.ToCurrencyID = @companyBaseCurrencyID        
         
            
      
  UNION All                  
                   
                      
      
  Select StanPair.ToCurrencyID AS FromCurrencyID,                    
      
    StanPair.FromCurrencyID AS ToCurrencyID,                    
      
    CCR.ConversionRate AS RateValue,                     
      
    1 AS ConversionMethod,                     
      
    CCR.Date AS Date,                    
      
    StanPair.eSignalSymbol AS eSignalSymbol,              
      
  Case               
      
  when datediff(d,CCR.Date,yesterdayDates.LatestAUECDate) = 0 then 0              
      
  else 1               
      
  End as InputDatePriceIndicator ,          
      
  Currency1.CurrencySymbol+'-'+Currency2.CurrencySymbol as TickerSymbol             
      
  from T_CurrencyConversionRate AS CCR                    
      
  INNER JOIN #TempCurrencyPairDates MaxCurrPair on CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairId And CCR.Date = MaxCurrPair.MaxDate                  
      
  INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId                  
      
  INNER JOIN #YesterdayAUECdatesTable yesterdayDates on MaxCurrPair.AUECID = yesterdayDates.AUECID        
      
  INNER JOIN T_Currency as Currency1 on Currency1.CurrencyID=StanPair.FromCurrencyID          
      
  INNER JOIN T_Currency as Currency2 on Currency2.CurrencyID=StanPair.ToCurrencyID          
          
        
      
       
      
 Select * from #FXConversionRatesForDate         
      
 WHERE ToCurrencyID = @companyBaseCurrencyID        
        
       
      
 Drop Table #TempCurrencyPairDates ,#FXConversionRatesForDate,#YesterdayAUECdatesTable,#T_AUEC,#T_Group       
        
END           
        
        
        