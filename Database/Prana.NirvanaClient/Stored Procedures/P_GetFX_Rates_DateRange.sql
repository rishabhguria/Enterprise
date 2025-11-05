/*********************************************          
Create Date: 06-October-2015          
Created By: Pankaj Sharma          
Decsription:  FX Rate for all Currencies for each day between Date Range        
      
exec P_GetFX_Rates_DateRange       
@StartDate='2015-07-01 00:00:00:000',      
@EndDate='2015-07-30 00:00:00:000',  
@CurrencyPairID=N'21,6,14,3,8,13,16,5,4,23,22,15,2,19,1,17,28,26,12,10'      
*********************************************/  
          
CREATE PROCEDURE [dbo].[P_GetFX_Rates_DateRange]          
(          
  @StartDate datetime,          
  @EndDate datetime,        
  @CurrencyPairID varchar(max)    
)          
As          
        
Select * Into #TempCurrencyPairID    
from dbo.Split(@CurrencyPairID, ',')        
          
Select            
C1.CurrencySymbol as FromCurrency,          
C2.CurrencySymbol as ToCurrency,          
ConversionRate as FXRate,          
date          
          
from T_CurrencyStandardPairs CTP          
INNER JOIN T_CurrencyConversionRate CCR on CTP.CurrencyPairID = CCR.CurrencyPairID_FK          
INNER JOIN T_currency C1 on CTP.FromCurrencyID = C1.CurrencyID           
INNER JOIN T_currency C2 on CTP.TOCurrencyID = C2.CurrencyID          
where          
datediff(d,date,@StartDate)<=0 and datediff(d,date,@EndDate)>=0     
And CTP.CurrencyPairID In (Select Items from #TempCurrencyPairID)         
          
order by date          
        
Drop table #TempCurrencyPairID