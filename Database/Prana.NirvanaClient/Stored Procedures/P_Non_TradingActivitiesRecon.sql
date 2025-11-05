
Create Procedure [dbo].[P_Non_TradingActivitiesRecon]   
(                                    
 @StartDate datetime,               
 @EndDate datetime,   
 @AssetIds VARCHAR(MAX),                              
 @FundIds VARCHAR(MAX)     
)    
                                                                                                                                 
As     


 Create Table #Funds (FundID int)                                                          
 if (@FundIds is NULL or @FundIds = '')                                                          
 Insert into #Funds                                                          
 Select CompanyFundID as FundID from T_CompanyFunds   
 Where IsActive=1                                                        
 else                                                          
 Insert into #Funds                                                          
 Select Items as FundID from dbo.Split(@FundIds,',')     


select 
max(T_CompanyFunds.FundName) as FundName 
,sum(dr-cr) as CashValueLocal
,name  as Symbol  
,max(isnull(pbdesc,' ')) as Description
,max(CurrencySymbol) as Currency
,Transactiondate as TradeDate
,'Non-Trading' as TransactionSource  

from T_Journal 
inner join #Funds  on #Funds.FundId = T_Journal.FundID  
inner join T_CompanyFunds on CompanyFundID=T_Journal.Fundid
inner join T_Currency on  T_Journal.currencyid=T_currency.currencyid
inner join T_Subaccounts on  T_Subaccounts.subaccountid=T_Journal.subaccountid

where transactionsource=2 
and acronym not like 'cash'
and Datediff(d,Transactiondate,@StartDate)<=0  and Datediff(d,Transactiondate,@EndDate)>=0 

group by transactionid ,transactiondate,Name



