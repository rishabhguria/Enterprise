 /************************************                    
Author : Pankaj Sharma    
Creation Date : 8 October,2015                    
Execution Method :               
P_groupingParameters_InterestIncomeExpenseReport  
                    
************************************/       
          
CREATE Proc P_groupingParameters_InterestIncomeExpenseReport    
AS        
        
Create table #Temp        
(        
items Varchar(100),        
Sort int        
        
)        
        
insert into #Temp Values('Select',0)    
insert into #Temp Values('SubCategoryName',1)    
insert into #Temp Values('Name',1)    
insert into #Temp Values('FundName',1)    
insert into #Temp Values('CurrencyName',1)    
insert into #Temp Values('TransactionDate',1)    
        
Select items from #Temp order by sort ,items        
        
drop table #Temp