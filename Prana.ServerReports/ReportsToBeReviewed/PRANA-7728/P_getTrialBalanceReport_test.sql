                    
Create PROCEDURE [dbo].[P_getTrialBalanceReport_test]                  
(                        
@Startdate datetime,                      
@Enddate datetime,                     
@fund varchar(max)                        
)                        
As                     
                    
                      
Select * Into #Funds                                                    
from dbo.Split(@fund, ',')                      
                  
                  
Select                
SUM(OpenDrBalBase) as OpenDR,              
SUM(OpenCrBalBase) as OpenCR    
              
into #IncomeExpense              
from T_SubAccountBalances SubBal                        
INNER JOIN T_SubAccounts SubAcc on SubBal.SubAccountID = SubAcc.SubAccountID                        
INNER JOIN T_SubCategory SubCat on SubAcc.SubCategoryID = SubCat.SubCategoryID                        
where         
datediff(d,TransactionDate,@Startdate)=0              
and         
(        
SubCat.MasterCategoryID  in (4,5)           
or        
SubAcc.Name in ('Cash Withdrawal','Cash Deposit')        
)        
and           
SubBal.FundID in (Select * from #Funds)               
              
                    
Select                  
 CF.FundName                   
,MastCat.MasterCategoryName                    
,SubCat.SubCategoryName    
,SubAcc.Name as SubAccountName    
,                   
CASE                   
WHEN (MastCat.MasterCategoryName = 'Income')                  
THEN 1                  
WHEN (MastCat.MasterCategoryName = 'Expenses')                  
THEN 2                  
WHEN (MastCat.MasterCategoryName = 'Asset')                  
THEN 3                  
WHEN (MastCat.MasterCategoryName = 'Liability')                  
THEN 4                  
WHEN (MastCat.MasterCategoryName = 'Cash')                  
THEN 5                  
WHEN (MastCat.MasterCategoryName = 'Equity')                  
THEN 6                  
Else 0                  
End as SortOrder                  
                    
,CASE                     
WHEN (DATEDIFF(d,SubBal.TransactionDate,@Enddate)=0)                    
Then CloseDrBalBase              
Else 0              
End as CloseDrBalBase              
              
,CASE                     
WHEN (DATEDIFF(d,SubBal.TransactionDate,@Enddate)=0)                    
Then CloseCrBalBase              
Else 0              
End as CloseCrBalBase              
              
,CASE                     
WHEN (DATEDIFF(d,SubBal.TransactionDate,@Startdate)=0)                    
Then OpenDrBalBase              
Else 0              
End as OpenDrBalBase              
              
,CASE                     
WHEN (DATEDIFF(d,SubBal.TransactionDate,@Startdate)=0)                    
Then OpenCrBalBase              
Else 0              
End as OpenCrBalBase              
                
,DayDrBase as DayDrBalBase              
,DayCrBase as DayCrBalBase                  
,Curr.CurrencySymbol as CurrencySymbol               
,TransactionDate              
                 
                    
into #TrialBalance                    
                    
from T_SubAccountBalances SubBal                        
INNER JOIN T_SubAccounts SubAcc on SubBal.SubAccountID = SubAcc.SubAccountID                        
INNER JOIN T_SubCategory SubCat on SubAcc.SubCategoryID = SubCat.SubCategoryID                        
INNER JOIN T_MasterCategory MastCat on SubCat.MasterCategoryID = MastCat.MasterCategoryID                    
inner JOIN T_Currency Curr on SubBal.CurrencyID = Curr.CurrencyID     
inner JOIN T_CompanyFunds CF ON SubBal.FundID=CF.CompanyFundID                 
                    
Where                     
DATEDIFF(d,@StartDate,SubBal.TransactionDate)>=0               
AND               
DATEDIFF(d,SubBal.TransactionDate,@Enddate)>=0                
And                     
SubBal.FundID in (Select * from #Funds)                
                
              
Select      
FundName,                 
MasterCategoryName,    
SubcategoryName,    
SubAccountName,    
SortOrder,      
CurrencySymbol,               
              
              
CASE                  
WHEN (MasterCategoryName in ('Income','Expenses') or SubAccountName in ('Cash Withdrawal','Cash Deposit'))              
THEN SUM(DayDrBalBase)        
Else SUM(CloseDrBalBase)            
End as CloseDrBalBase,              
                   
              
CASE                  
WHEN (MasterCategoryName in ('Income','Expenses') or SubAccountName in ('Cash Withdrawal','Cash Deposit'))              
THEN SUM(DayCrBalBase)                 
Else SUM(CloseCrBalBase)                  
End as CloseCrBalBase,              
               
                 
SUM                  
(                  
CASE                  
WHEN (MasterCategoryName in ('Income','Expenses') or SubAccountName in ('Cash Withdrawal','Cash Deposit'))              
THEN 0                  
Else OpenDrBalBase                  
End                  
) as OpenDrBalBase,               
              
                   
SUM                  
(                  
CASE                  
WHEN (MasterCategoryName in ('Income','Expenses') or SubAccountName in ('Cash Withdrawal','Cash Deposit'))              
THEN 0                  
Else OpenCrBalBase                  
End                  
) as OpenCrBalBase,                
              
                  
SUM(DayDrBalBase) as DayDrBalBase,                    
SUM(DayCrBalBase) as DayCrBalBase                
                 
into #AllData                  
from #TrialBalance                    
Group by FundName,SortOrder,MasterCategoryName,SubCategoryName,SubAccountName,CurrencySymbol                    
              
              
insert into #AllData              
Select     
'' As FundName,              
'Equity' as MasterCategoryName,      
'Equity'as SubCategoryName,      
'Retained Earnings - Period Start' as SubAccountName,               
6 as SortOrder,                  
'USD' as CurrencySymbol,              
OpenDR AS CloseDrBalBase,              
OpenCR as CloseCrBalBase,            
        
--0.00 AS CloseDrBalBase,              
--0.00 as CloseCrBalBase,        
          
OpenDR as OpenDrBalBase,              
OpenCR as OpenCrBalBase,              
0.00 as DayDrBalBase,              
0.00 as DayCrBalBase              
              
From #IncomeExpense              
              
               
             
              
select               
FundName,             
MasterCategoryName,    
SubCategoryName,    
SubAccountName,               
SortOrder,              
CurrencySymbol,              
SUM(CloseDrBalBase) as CloseDrBalBase,              
SUM(CloseCrBalBase) as CloseCrBalBase,              
SUM(OpenDrBalBase) as OpenDrBalBase,              
SUM(OpenCrBalBase) as OpenCrBalBase,              
SUM(DayDrBalBase) as DayDrBalBase,              
SUM(DayCrBalBase) as DayCrBalBase              
from #AllData               
group by FundName,SortOrder,MasterCategoryName,SubCategoryName,SubAccountName,CurrencySymbol              
order by FundName,SortOrder,MasterCategoryName,SubCategoryName,SubAccountName,CurrencySymbol     
              
                    
Drop table #Funds,#TrialBalance,#IncomeExpense,#AllData  