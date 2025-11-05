/*********************************************                                  
Create Date: 22-October-2015                                  
Created By: Pankaj Sharma                                  
Decsription:  We are giving breakdown of values available in PnL Summary Report on daily basis.        
Execution Statement:                
                
exec P_UnrealizedGainLossOnEODCash              
@StartDate = '06-30-2015',            
@EndDate = '07-31-2015',            
@Currency=N'3,6,7,1,4,2,16,14,8'                            
*************************************************/                                  
                                            
CREATE Procedure [dbo].[P_UnrealizedGainLossOnEODCash]                          
(                            
 @StartDate datetime                          
,@EndDate datetime                                  
,@Currency varchar(max)                           
)                            
As                     
        
Declare @AUECID int        
select top 1 @AUECID = DefaultAUECID  from T_company        
      
Declare @BaseCurrencyID int      
Select Top 1 @BaseCurrencyID = BaseCurrencyID from T_Company      
              
Declare @PrevMonthDate datetime              
Select @PrevMonthDate =  dbo.AdjustBusinessDays(@StartDate,-1,@AUECID)          
                  
Select * Into #TempCurrency                
from dbo.Split(@Currency, ',')                               
                   
-------------------------------FX Convension-------------------------------------------------                                            
DECLARE  @FXConversionRates Table                            
(                            
 FromCurrencyID int,              
 ToCurrencyID int,                            
 RateValue float,                            
 ConversionMethod int,                            
 Date DateTime,                            
 eSignalSymbol varchar(max)                            
)              
              
Insert into @FXConversionRates                                                    
Exec P_GetAllFXConversionRatesForGivenDateRange @PrevMonthDate,@EndDate                            
--  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@StartDate,@EndDate) as A                                                                                                                                                                      
  
    
UPDATE @FXConversionRates                            
Set RateValue = 1.0/RateValue                            
Where RateValue <> 0 and ConversionMethod = 1                            
                                                                                                                                 
UPDATE @FXConversionRates                                                                                                                                                                         
Set RateValue = 0                                                                                                    
Where RateValue is Null                                                  
              
--select * from @FXConversionRates                                            
--SELECT * from @FXConversionRates WHERE conversionMethod ='1'order by date                                            
------------------------------------------------------------------------------------------------------------                                            
------------------------------------------------------------------------------------------------------------                                            
              
Create Table #TempOpeningBalance              
(          
CurrencyID int,          
CurrencyName varchar(max),          
Amount float,          
AmountPrevDay float,          
TransactionDate datetime          
)              
              
insert into #TempOpeningBalance(CurrencyID,CurrencyName,Amount,AmountPrevDay,TransactionDate)              
select               
SACCTemp.CurrencyID,          
CUR.CurrencySymbol,          
Round(Sum(CloseDRBal-CloseCRBal),2),          
Round(Sum(OpenDRBal-CloseCRBal),2),          
TransactionDate          
from T_SubAccountBalances SACCTemp          
INNER JOIN T_currency CUR on CUR.CurrencyID  = SACCTemp.CurrencyID 
INNER JOIN #TempCurrency TempCurr on CUR.CurrencyID  = TempCurr.items
where             
SubAccountID =17           
and          
datediff(d,@StartDate,TransactionDate)>=0          
and          
datediff(d,TransactionDate,@EndDate)>=0         
and SACCTemp.CurrencyID<> @BaseCurrencyID      
         
          
group by SACCTemp.CurrencyID,TransactionDate,CUR.CurrencySymbol          
            
              
------------------------------------------------------------------------------------------------------------                                            
------------------------------------------------------------------------------------------------------------               
alter table #TempOpeningBalance          
add FxratePrevDay float,          
FxrateCurrDay float,          
FxGainLoss float          
          
Update #TempOpeningBalance set FxratePrevDay = TempFX_Rates1.RateValue,          
FxrateCurrDay =TempFX_Rates2.RateValue           
from #TempOpeningBalance TOB          
LEFT JOIN @FXConversionRates TempFX_Rates1 on TempFX_Rates1.FromCurrencyID = TOB.CurrencyID and  DateDiff(d,TempFX_Rates1.Date,TOB.TransactionDate)=1                  
LEFT JOIN @FXConversionRates TempFX_Rates2 on TempFX_Rates2.FromCurrencyID = TOB.CurrencyID and  TempFX_Rates2.Date = TOB.TransactionDate              
          
Update #TempOpeningBalance set FxGainLoss = (FxrateCurrDay-FxratePrevDay)*AmountPrevDay          
          
          
select * from #TempOpeningBalance            
          
Drop table #TempOpeningBalance