/*********************************************          
Create Date: 08-October-2015          
Created By: Pankaj Sharma          
Decsription:  Get SubCategory for filters in the report        
      
exec P_GetSubCategory_InterstIncomeExpense  
      
*********************************************/                          
          
CREATE PROCEDURE [dbo].[P_GetSubCategory_InterstIncomeExpense]    
          
As          
Create Table #Temp_SubCategory    
(    
SubCategoryName varchar(100)    
)    
insert into #Temp_SubCategory values('Interest Expense')    
insert into #Temp_SubCategory values('Interest Income')    
select * from  #Temp_SubCategory    
Drop table #Temp_SubCategory 