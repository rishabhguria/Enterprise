                          
CREATE PROC [dbo].[P_GetDividendRecon_Open]                          
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
                     
  Select Div.CashTransactionId, Funds.FundName as AccountName,sec.CompanyName as SecurityName, Div.TaxlotId, Div.Symbol, sec.SEDOLSymbol,sec.ISINSymbol ,Div.Amount as Dividend,                    
  Div.ExDate,Div.PayoutDate,Div.RecordDate,Div.DeclarationDate, CUR.CurrencySymbol as Currency,                       
  taxlots.CorpActionId, taxlots.ParentRow_Pk , V_corpactiondata.DivRate,Div.Description,BloombergSymbol as BloombergSYmbol                      
  from T_CashtRANSACTIONS Div Left outer join PM_CorpActiontaxlots taxlots on Div.CashTransactionId = taxlots.FKId                      
  Inner join #Funds on Div.FundID = #Funds.FundID                  
  Left outer join V_corpactiondata on V_corpactiondata.CorpActionId=taxlots.CorpActionId                   
  Left outer join V_Secmasterdata sec on sec.TickerSymbol=div.Symbol                   
  Left outer join T_Currency CUR on CUR.CurrencyID=Div.CurrencyID                             
  inner join T_CompanyFunds Funds on Div.FundId = Funds.CompanyFundId                      
  where                   
--datediff(d,Div.ExDate,@StartDate)<=0 and                   
datediff(d,Div.Payoutdate,@EndDate)<0 and            
--datediff(d,Div.ExDate,@StartDate)<=0 and              
datediff(d,Div.exdate,@EndDate)>=0       
and activitytypeid in (select activitytypeid from t_Activitytype where Acronym like 'DividendIncome' or  Acronym like 'DividendExpense')        
                    
 End