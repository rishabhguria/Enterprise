  /************************************                          
Modified Date: 17-August-2015                                                                                        
Modified By : Pankaj Sharma     
Base Sp (P_MW_groupingParameters_VSR)    
Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-10311
Description:     
1) Grouping is required for only Fund and MasterFund    
    
    
Usage:                        
exec P_MW_groupingParameters_CustomizedVSR    
                          
************************************/             
                
CREATE Proc P_MW_groupingParameters1_CustomizedVSR    
--(              
--@IncludeBloomberg char,              
--@IncludeSedol char              
--)              
AS              
              
Create table #Temp              
(              
items Varchar(100),              
Sort int              
              
)              
              
insert into #Temp Values('Select',0)              
        
--insert into #Temp Values('Asset',1)         
--insert into #Temp Values('UDACountry',1)         
insert into #Temp Values('Fund',1)         
insert into #Temp Values('MasterFund',1)        
--insert into #Temp Values('UDASector',1)        
--insert into #Temp Values('UDASecurityType',1)         
--insert into #Temp Values('Side',1)        
--insert into #Temp Values('strategy',1)        
--insert into #Temp Values('Symbol',1)               
--insert into #Temp Values('UDASubSector',1)              
--insert into #Temp Values('UnderlyingSymbol',1)           
--insert into #Temp Values('TradeCurrency',1)         
--insert into #Temp Values('OpenTradeAttribute1',1)           
--insert into #Temp Values('OpenTradeAttribute2',1)           
--insert into #Temp Values('OpenTradeAttribute3',1)           
--insert into #Temp Values('OpenTradeAttribute4',1)           
--insert into #Temp Values('OpenTradeAttribute5',1)           
--insert into #Temp Values('OpenTradeAttribute6',1)             
              
              
Select items from #Temp order by sort ,items              
              
drop table #Temp