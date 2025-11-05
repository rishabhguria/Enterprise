/*********************************************          
Create Date: 08-October-2015          
Created By: Pankaj Sharma          
Decsription:  Get Name on basis of SubCategories for filters in the report        
      
    
exec P_GetSubAccountName_InterestIncomeExpense @SubCategory=N'Interest Expense,Interest Income'    
      
*********************************************/                          
          
CREATE PROCEDURE [dbo].[P_GetSubAccountName_InterestIncomeExpense]    
(    
@SubCategory varchar(max)    
)    
          
As          
    
Select * Into #TempSubCategory    
from dbo.Split(@SubCategory, ',')      
    
select distinct [Name],SAcc.SubAccountID from T_SubAccounts SAcc    
inner Join  T_SubCategory SCat on SCat.SubCategoryID = SAcc.SubCAtegoryID     
inner Join T_MasterCategory MCat on MCat.MasterCategoryID = SCat.MasterCategoryID    
inner join T_journal JRNL on JRNL.SubaccountID=SACC.SubaccountID    
where    
(    
(MCat.MasterCategoryID in ('3','4','5') and TransactionTypeID<>3)                
or       
(TransactionTypeID=3 and (TransactionSource in (2,6)))    
)    
and     
SCat.SubCategoryName IN (Select items from #TempSubCategory)    
    
Drop table #TempSubCategory