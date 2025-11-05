/*********************************************                        
Create Date: 09-October-2015                        
Created By: Pankaj Sharma                        
Decsription:  We are giving breakdown of the difference in PSR and PNL Summary Report      
Execution Statement:      
      
exec P_UnrealizedGainonAccruals    
@StartDate = '06-30-2015',      
@EndDate = '07-31-2015',      
@Currency=N'3,6,7,1,4,2,16,14,8'                  
*************************************************/                        
                                  
CREATE Procedure [dbo].[P_UnrealizedGainonAccruals]                  
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
  
Declare @Description varchar(100)  
Set @Description = 'Opening Balance as of '+CONVERT(VARCHAR(2),DATEPART(MONTH, @PrevMonthDate)) + '/'+ CONVERT(VARCHAR(2),DATEPART(DAY, @PrevMonthDate))   
        
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
Amount float,    
TransactionDate datetime    
)    
    
insert into #TempOpeningBalance(CurrencyID,Amount,TransactionDate)    
select     
CurrencyID,    
Sum(CloseDRBal-CloseCRBal) as Amount1,    
TransactionDate    
from T_SubAccountBalances where CurrencyID<>@BaseCurrencyID
and     
TransactionDate= @PrevMonthDate     
and SubAccountID in(select distinct SubAccountID from T_subAccounts where TransactionTypeID=2)    
group by CurrencyID,TransactionDate    
    
------------------------------------------------------------------------------------------------------------                                  
------------------------------------------------------------------------------------------------------------     
Create Table #FinalData    
(    
CurrencyName varchar(max),      
Amount float,    
Name varchar(max),      
TransactionDate datetime,                  
TradePrice float,                  
FX_Rate float    
)    
-----------------------insert previous month Opening Balances    
insert into #FinalData(CurrencyName,Amount,SACC.Name,TransactionDate,TradePrice,FX_Rate)    
    
select     
CUR.CurrencySymbol as CurrencyName,    
Amount,    
@Description,    
TransactionDate,    
TempFX_Rates1.RateValue,    
TempFX_Rates2.RateValue    
    
from #TempOpeningBalance SACCTemp    
INNER JOIN T_currency CUR on CUR.CurrencyID  = SACCTemp.CurrencyID    
LEFT JOIN @FXConversionRates TempFX_Rates1 on TempFX_Rates1.FromCurrencyID = SACCTemp.CurrencyID and  TempFX_Rates1.Date = SACCTemp.TransactionDate    
LEFT JOIN @FXConversionRates TempFX_Rates2 on TempFX_Rates2.FromCurrencyID = SACCTemp.CurrencyID and  DateDiff(d,TempFX_Rates2.Date,@EndDate)=0    
    
    
-----------------------insert Current month Accruals    
    
insert into #FinalData(CurrencyName,Amount,SACC.Name,TransactionDate,TradePrice,FX_Rate)    
    
Select     
CUR.CurrencySymbol as CurrencyName,    
(DR-CR) as Amount,    
SACC.Name,      
TransactionDate,    
TempFX_Rates1.RateValue,    
TempFX_Rates2.RateValue    
                               
from t_journal JRNL    
INNER JOIN #TempCurrency TC on TC.items = JRNL.CurrencyID        
INNER JOIN T_currency CUR on CUR.CurrencyID = JRNL.CurrencyID    
INNER JOIN T_SubAccounts SACC on SACC.SubAccountID = JRNL.SubAccountID                  
LEFT JOIN @FXConversionRates TempFX_Rates1 on TempFX_Rates1.FromCurrencyID = JRNL.CurrencyID and  TempFX_Rates1.Date = JRNL.TransactionDate    
LEFT JOIN @FXConversionRates TempFX_Rates2 on TempFX_Rates2.FromCurrencyID = JRNL.CurrencyID and  DateDiff(d,TempFX_Rates2.Date,@EndDate)=0    
    
                                  
where                                  
datediff(d,@StartDate,TransactionDate)>=0                                  
and                                  
datediff(d,TransactionDate,@EndDate)>=0                      
and       
JRNL.SubAccountID in(select distinct SubaccountID from T_SubAccounts where TransactionTypeID=2)              
and      
TransactionSource in (2,5)            
and    
JRNL.CurrencyID <> @BaseCurrencyID
    
select * from #FinalData order by TransactionDate, CurrencyName    
    
Drop table #TempOpeningBalance,#FinalData    
------------------------------------------------------------------------------------------------------------ 