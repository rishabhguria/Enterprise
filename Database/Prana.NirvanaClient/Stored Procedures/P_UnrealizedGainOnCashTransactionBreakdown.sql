/*********************************************                          
Create Date: 09-October-2015                          
Created By: Pankaj Sharma                          
Decsription:  We are giving breakdown of the value available in PSR(TESTING)                    
Execution Statement:                                                 
                        
exec P_UnrealizedGainOnCashTransactionBreakdown                     
@StartDate = '07-1-2015',                    
@EndDate = '07-31-2015',                  
@fund=N'1254,1252,1253,1239,1213,1251,1214,1255,1238,1250,1247,1248,1249,1240,1241,1256',                            
@Currency=N'3,6,7,1,4,2,16,14,8'                    
*************************************************/                          
                                    
CREATE Procedure [dbo].[P_UnrealizedGainOnCashTransactionBreakdown]                    
(                    
@StartDate datetime                  
,@EndDate datetime                  
,@fund varchar(max)                  
,@Currency varchar(max)                   
)                    
As             
          
Select * Into #Funds                                                                      
from dbo.Split(@fund, ',')              
        
Declare @BaseCurrencyID int        
Select Top 1 @BaseCurrencyID = BaseCurrencyID from T_Company                 
           
Create table #TempCurrency (CurrencyID_Filtered int)          
insert into #TempCurrency           
Select * from dbo.Split(@Currency, ',')                   
                    
                    
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
Exec P_GetAllFXConversionRatesForGivenDateRange @StartDate,@EndDate                    
--  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@StartDate,@EndDate) as A                                                                                                                                                                   
                                                                                            
UPDATE @FXConversionRates                    
Set RateValue = 1.0/RateValue                    
Where RateValue <> 0 and ConversionMethod = 1                    
                                                                                                                         
UPDATE @FXConversionRates                                                                                                                                                                 
Set RateValue = 0                                                                                            
Where RateValue is Null                                          
                                    
--SELECT * from @FXConversionRates WHERE conversionMethod ='1'order by date                                    
------------------------------------------------------------------------------------------------------------                                    
------------------------------------------------------------------------------------------------------------                                    
                                    
Select *                                     
into #JournalPNL                                    
from t_journal          
INNER JOIN #Funds TempFunds on FundID =  TempFunds.items          
INNER JOIN #TempCurrency TC on TC.CurrencyID_Filtered = t_journal.CurrencyID          
       
where                                    
datediff(d,@StartDate,TransactionDate)>=0                                    
and                             
datediff(d,TransactionDate,@EndDate)>=0    
and      
(        
 (        
  subAccountID IN (17) And                            
  (        
   (CharIndex('Spot',PBDesc) <> 0 and taxlotid='')                     
   or                     
   (transactionsource In (7) and  CharIndex('Spot',symbol) <> 0 )                    
  )         
 )        
        
Or        
 (        
  subAccountID IN (17,135) And  taxlotid <> ''                     
   And transactionsource In (9) --And PBDESC <> 'Cash to Unrealized FXRate PNL Entry'                
  )        
)         
and                     
CurrencyID <> @BaseCurrencyID          
    
Update #JournalPNL set Symbol ='FX Forward Settlement (SPOT)'                     
where (Symbol IS NULL OR Symbol='') and PBDESC = 'FX Forward Settlement (SPOT)'                    
                    
               
Alter table #JournalPNL                    
add                     
Rate float                    
,Amount float                                    
                                    
UPdate #JournalPNL                 
Set Rate = RateValue                                    
from @FXConversionRates                                     
WHERE ToCurrencyID = '1'                                     
and DateDiff(Day,Date,TransactionDate) = 0                                     
and CurrencyID = FromCurrencyID                                    
                                    
Update #JournalPNL                                    
Set                                    
Amount =(DR-CR)     
                         
              
select     
TransactionDate,                    
CUF.FundName,                    
Symbol,                    
CUR.CurrencySymbol as CurrencyName,                    
SACC.Name,                    
Amount,                       
FxRate as TradePrice,                    
Rate as FX_Rate,                    
(Rate-FxRate)*Amount as UnrealizedGainOnCashTransaction                    
into #FinalData    
from #JournalPNL JRNL                    
INNER JOIN T_CompanyFunds CUF on CUF.CompanyFundID = JRNL.FundID                    
INNER JOIN T_currency CUR on CUR.CurrencyID = JRNL.CurrencyID                    
INNER JOIN T_SubAccounts SACC on SACC.SubAccountID = JRNL.SubAccountID                       
order by TransactionDate    
  
Select * from #FinalData  
    
Drop table #JournalPNL, #FinalData