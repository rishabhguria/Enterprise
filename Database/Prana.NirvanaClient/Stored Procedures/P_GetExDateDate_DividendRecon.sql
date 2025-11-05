    /*
Author: Kuldeep Kumar
Created Date: 21 Nov 2024
Description: Requirement to fetch the ExDate dividend for Dividend Recon.
*/

CREATE PROC [dbo].[P_GetExDateDate_DividendRecon]                          
(                            
 @StartDate datetime,                          
 @EndDate datetime,                    
 @AssetIds varchar(MAX),                      
 @FundIds varchar(MAX)                           
)   

--Declare
-- @StartDate datetime,                          
-- @EndDate datetime,                    
-- @AssetIds varchar(MAX),                      
-- @FundIds varchar(MAX) 

--set @StartDate='2024-11-08 00:00:00'
--set @EndDate='2024-11-20 00:00:00'
--set @AssetIds=N''
--set @FundIds=N''  
                     
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
  ,Div.Symbol as Symbol 
  ,Div.Amount as Dividend                   
  ,Div.ExDate as ExDate
  ,Div.PayoutDate as PayoutDate
  ,Div.RecordDate as RecordDate 
  ,CUR.CurrencySymbol as Currency                       
  ,Div.[Description] as [Description]  
  ,ACT.Acronym as ActivityType                      
  from T_CashtRANSACTIONS Div With (NoLock)                    
  Inner join #Funds on Div.FundID = #Funds.FundID                     
  inner join T_Currency CUR With (NoLock) on CUR.CurrencyID=Div.CurrencyID                             
  inner join T_CompanyFunds Funds With (NoLock) on Div.FundId = Funds.CompanyFundId 
  Inner join t_Activitytype ACT With (NoLock) on Div.ActivityTypeId= ACT.ActivityTypeId                     
  where                 
datediff(Day,@StartDate,Div.ExDate)>=0
and DateDiff(Day,Div.ExDate,@EndDate)>=0
and Div.activitytypeid in (select activitytypeid from t_Activitytype 
   where Acronym like 'DividendIncome' or Acronym like 'DividendExpense' or Acronym like 'WithholdingTax')        
  
  drop table #Funds                    
 End

