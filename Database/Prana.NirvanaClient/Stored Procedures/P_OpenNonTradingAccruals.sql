

Create Procedure P_OpenNonTradingAccruals
(                      
 @StartDate datetime,                    
 @EndDate datetime,              
 @AssetIds varchar(MAX),                
 @FundIds varchar(MAX)                     
)                    
As                   
 Begin  

 Create Table #Funds (FundID int)                                                    
 if (@FundIds is NULL or @FundIds = '')                                                    
 Insert into #Funds                                                    
 Select CompanyFundID as FundID from T_CompanyFunds Where IsActive=1                                                   
 else                                                    
 Insert into #Funds                                                    
 Select Items as FundID from dbo.Split(@FundIds,',')            
   


select 
b.ActivityType as ActivityType
,T_CompanyFunds.FundName as FundName 
,Amount as CashValueLocal
,Symbol  as Symbol  
,isnull(a.Description,' ') as Description
,CurrencySymbol as Currency
,ExDate as TradeDate
,PayoutDate as SettlementDate
,'Non-Trading' as TransactionSource  


From T_CashTransactions a 
inner join t_Activitytype b  on a.ActivityTypeId=b.activitytypeid
inner join #Funds  on #Funds.FundId = a.FundID  
inner join T_CompanyFunds on CompanyFundID=a.Fundid
inner join T_Currency on  a.currencyid=T_currency.currencyid

where 
b.activitysource<>2
and a.activitytypeid in (select Activitytypeid_fk From t_activityjournalmapping where activityDateType=2)
and              
datediff(d,Payoutdate,@EndDate)<0 and datediff(d,exdate,@EndDate)>=0    

END

