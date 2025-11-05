                        
CREATE PROC [dbo].[P_GetPayoutDate_DividendRecon]                          
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
 Select CompanyFundID as FundID 
 from T_CompanyFunds Where IsActive=1                                                          
 else                                                          
 Insert into #Funds                                                          
 Select Items as FundID from dbo.Split(@FundIds,',')                  
                     
  Select 
   Funds.FundName as AccountName   
  ,Div.Symbol  
  ,Div.Amount as Dividend                   
  ,Div.ExDate
  ,Div.PayoutDate
  ,Div.RecordDate
  ,Div.DeclarationDate
  ,CUR.CurrencySymbol as Currency                       
  ,Div.[Description] as [Description]  
  ,ACT.Acronym as ActivityType                      
  from T_CashtRANSACTIONS Div 
                    
  Inner join #Funds on Div.FundID = #Funds.FundID                     
  inner join T_Currency CUR on CUR.CurrencyID=Div.CurrencyID                             
  inner join T_CompanyFunds Funds on Div.FundId = Funds.CompanyFundId 
  Inner join t_Activitytype ACT on Div.ActivityTypeId= ACT.ActivityTypeId                     
  where                 
datediff(d,Div.Payoutdate,@EndDate)=0 
and Div.activitytypeid in (select activitytypeid from t_Activitytype 
   where Acronym like 'DividendIncome' or Acronym like 'DividendExpense' or Acronym like 'WithholdingTax')        
                    
 End