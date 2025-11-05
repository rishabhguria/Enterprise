/************************************            
Author: Ankit Misra            
Date:   23 Jan. 2015              
Desc:   Return only the Broker that were traded by the client.             
Exec:   P_GetAllCounterPartyDynamically_Activity            
*************************************/            
            
            
CREATE  procedure [dbo].[P_GetAllCounterPartyDynamically_Activity]            
AS             
BEGIN            
 Create Table #TempBroker            
  (            
   Broker Varchar(100)            
  )            
----------------------------------------------------------------------------            
-- FILL UDASECTOR WHICH ARE PRESENT IN T_MW_Transactions TABLE            
----------------------------------------------------------------------------                          
 Insert Into #TempBroker            
  Select Distinct            
  CounterParty                        
  From T_MW_Transactions Transactions                   
  Order by CounterParty            
----------------------------------------------------------------------------            
-- IF NO DATA IN T_MW_Transactions SET UDASECTOR AS "No Data"            
----------------------------------------------------------------------------            
 If(Select Count(*) from #TempBroker) = 0            
 BEGIN            
  Insert into #TempBroker Values('No Data')            
 END            
             
 Select * from #TempBroker            
 Drop Table #TempBroker            
END 