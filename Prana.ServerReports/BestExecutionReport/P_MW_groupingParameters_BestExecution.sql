  /************************************                                
                                
Author : Pankaj Sharma                               
Creation Date : 14 July,2015            
Execution Method :                           
P_MW_groupingParameters_BestExecution '1','1'                         
                                
************************************/                   
                      
CREATE Proc P_MW_groupingParameters_BestExecution                     
AS                    
                    
Create table #Temp                    
(                    
items Varchar(100),                    
Sort int                    
                    
)                    
insert into #Temp Values('Select',0)                    
insert into #Temp Values('Fund',2)               
insert into #Temp Values('CounterParty',1)               
insert into #Temp Values('Side',4)              
insert into #Temp Values('Symbol',5)              
insert into #Temp Values('MasterFund',3)        
      
Select items from #Temp Order BY Sort          
                    
drop table #Temp