          
Create Procedure [dbo].[P_CashActivitiesRecon]             
(                                              
 @StartDate datetime,                         
 @EndDate datetime,             
 @AssetIds VARCHAR(MAX),                                        
 @FundIds VARCHAR(MAX)               
)              
                                                                                                                                           
As               
          
--- Activity Source Options------------------          
--  NonTrading = 0,          
--        Trading = 1,          
--        Dividend = 2,          
--        Revaluation =3,          
--        OpeningBalance = 4          
---------------------------------------------          
          
Declare @activitysource int          
set @activitysource=0          
          
 Create Table #Funds (FundID int)                                                                    
 if (@FundIds is NULL or @FundIds = '')                                                                    
 Insert into #Funds                                                                    
 Select CompanyFundID as FundID from T_CompanyFunds Where IsActive=1                                                                  
 else                                                                    
 Insert into #Funds                                                                    
 Select Items as FundID from dbo.Split(@FundIds,',')               
          
    select Activitytypeid_fk into #nontradingaccrualActivity From t_activityjournalmapping where activityDateType=2      
          
select           
(Amount -(+Commission+SoftCommission+OtherBrokerFees+ClearingBrokerFee+StampDuty +TransactionLevy +ClearingFee +TaxOnCommissions+MiscFees +SecFee+OccFee+OrfFee)  )as CashValuelocal           
,T_CompanyFunds.FundName as FundName           
,isnull(Symbol,'Undefined') as Symbol            
--,isnull(AssetName,'Undefined')  as Asset           
,ActivityType           
,CurrencySymbol as Currency           
,TradeDate as TradeDate          
,SettlementDate as SettlementDate          
,isnull(T_AllActivity.Description,' ') as Description           
,case           
when TransactionSource=1 then 'Trading'           
when TransactionSource=2 then 'Non-Trading' 
when TransactionSource=3 then 'Daily Calculation'   
when TransactionSource=4 then 'Corporate Action'                 
when TransactionSource=5 then 'Cash Transactions'
when TransactionSource=6 then 'Importable & Editable Data' 
when TransactionSource=7 then 'Closing'           
when TransactionSource=8 then 'Opening Balance'           
when TransactionSource=9 then 'Revaluation'           
else 'Not Known' end as TransactionSource           
from T_AllActivity inner join T_CompanyFunds on CompanyFundID=Fundid          
inner join #Funds    on #Funds.FundId = T_AllActivity.FundID            
inner join T_currency on  T_AllActivity.currencyid=T_currency.currencyid          
inner join T_ActivityType on Activitytypeid=Activitytypeid_fk 
left outer join #nontradingaccrualActivity a on a.Activitytypeid_fk= T_ActivityType.ActivityTypeId 
         
      
where T_AllActivity.BalanceType=1      
  and         
(        
  ( a.activitytypeid_fk is null and Datediff(d,TradeDate,@StartDate)<=0  and Datediff(d,TradeDate,@EndDate)>=0  )         
  or 
  ( a.activitytypeid_fk is not null and Datediff(d,settlementdate,@StartDate)<=0  and Datediff(d,settlementdate,@EndDate)>=0  )                
)        
