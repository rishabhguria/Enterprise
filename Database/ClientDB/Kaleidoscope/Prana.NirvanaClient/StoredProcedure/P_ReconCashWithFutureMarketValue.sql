CREATE PROCEDURE [dbo].[P_ReconCashWithFutureMarketValue]                                                                                                          
(                                                                                                                                                                                
 @StartDate datetime,                                 
 @EndDate datetime,                                                
 @AssetIds VARCHAR(MAX),                                                
 @FundIds VARCHAR(MAX)                                                                                                                                                                                                                   
)                                                                                                                                                                                
As                                                          
Begin                                    
                                  
              
 Create Table #Funds (FundID int)     
                                                                           
 if (@FundIds is NULL or @FundIds = '')                                                                            
 Insert into #Funds                                                                            
 Select CompanyFundID as FundID from T_CompanyFunds                                                                            
 else                                                                            
 Insert into #Funds                                                                            
 Select Items as FundID from dbo.Split(@FundIds,',')                                                                            
           
    
    
--create table #temptable    
--(    
--fundid int,    
--currencyid int,    
--Symbol varchar(200),    
--Quantity float,    
--markprice float,    
--MarkPriceBase float,    
--FundName varchar(200),    
--MarketValue float,    
--MarketValueBase float,    
--FXRate float    
--)    
    
create table temptable1    
(    
fundid int,    
currencyid int,    
Symbol varchar(200),    
Quantity float,    
markprice float,    
MarkPriceBase float,    
FundName varchar(200),    
MarketValue float,    
MarketValueBase float,    
FXRate float    
)    
    
         
    
    
                       
exec [PMGetFundPositionsWithMarketValue_newforSP1] @StartDate,@AssetIds,@FundIds                                                                        
--select * FRom temptable1    
    
--select fundid,currencyid,sum(MarketValue) as MarketValue,sum(MarketValueBase) as MarketValueBase,@StartDate as transactiondate  from  temptable1                 
--group by fundid,currencyid       
    
    
    
    
                                  
select                                   
CF.FundName as AccountName,                                   
isnull(CompanyFundCashCurrencyValue.Date,FuturesMarketValue.transactiondate) as TradeDate,                                   
CurrencyLocal.CurrencySymbol as Symbol,                                      
isnull(CompanyFundCashCurrencyValue.CashValueLocal,0) as CashValueLocal,                               
isnull(CompanyFundCashCurrencyValue.CashValueBase,0) as CashValueBase,                
isnull(Marketvalue,0) AS Marketvalue,              
isnull(MarketvalueBase,0)  AS MarketvalueBase                        
from #Funds              
left join PM_CompanyFundCashCurrencyValue CompanyFundCashCurrencyValue                  
on #Funds.FundId = CompanyFundCashCurrencyValue.FundID                       
full outer join              
       
(           
select fundid,currencyid,sum(MarketValue) as MarketValue,sum(MarketValueBase) as MarketValueBase,@StartDate as transactiondate  from  temptable1          
           
group by fundid,currencyid           
)FuturesMarketValue               
              
on FuturesMarketValue.fundid= CompanyFundCashCurrencyValue.fundid              
and  FuturesMarketValue.currencyid= CompanyFundCashCurrencyValue.LocalCurrencyID                
and  FuturesMarketValue.transactiondate= CompanyFundCashCurrencyValue.date                
              
                              
inner join T_CompanyFunds CF -- on CF.fundid=#funds.fundid               
              
on (cf.CompanyFundID = FuturesMarketValue.fundid or cf.CompanyFundID = CompanyFundCashCurrencyValue.FundID )               
                                
inner join T_Currency CurrencyLocal                                   
on (CurrencyLocal.CurrencyId = CompanyFundCashCurrencyValue.LocalCurrencyID or CurrencyLocal.CurrencyId= FuturesMarketValue.currencyid )                             
                              
where (datediff(d, CompanyFundCashCurrencyValue.Date, @StartDate)=0   or datediff(d, FuturesMarketValue.transactiondate, @StartDate)=0  )             
and   not ( isnull(Marketvalue,0)=0 and isnull(CompanyFundCashCurrencyValue.CashValueBase,0)=0 )                        
                               
order by fundname,COALESCE(CompanyFundCashCurrencyValue.Date ,FuturesMarketValue.transactiondate), CurrencyLocal.CurrencySymbol                           
                                                   
          
    
    
drop table temptable1    
                            
End