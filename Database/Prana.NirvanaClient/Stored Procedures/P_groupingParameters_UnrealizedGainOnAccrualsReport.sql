 /************************************                      
Author : Pankaj Sharma      
Creation Date : 22 October,2015                      
Execution Method :                 
P_groupingParameters_UnrealizedGainOnAccrualsReport    
                      
************************************/         
            
CREATE Proc P_groupingParameters_UnrealizedGainOnAccrualsReport  
AS          
          
Create table #Temp          
(          
items Varchar(100),          
Sort int          
          
)          
          
insert into #Temp Values('Select',0)  
insert into #Temp Values('CurrencyName',1)  
insert into #Temp Values('TransactionDate',1)  
          
Select items from #Temp order by sort ,items          
          
drop table #Temp