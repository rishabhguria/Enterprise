/************************************            
Author: Ankit Misra            
Date:   15 Jan. 2015              
Desc:   Return only the UDASector that were traded by the client.             
Exec:   [P_GetTransactionTypesDynamically_Activity]            
*************************************/            
            
            
CREATE  procedure [dbo].[P_GetTransactionTypesDynamically_Activity]                    
As             
BEGIN            
 Create Table #TransactionTypes            
  (            
   TransactionType Varchar(100)            
  )            
----------------------------------------------------------------------------            
-- FILL UDASECTOR WHICH ARE PRESENT IN T_MW_Transactions TABLE             
----------------------------------------------------------------------------                          
 Insert Into #TransactionTypes            
  Select Distinct            
  ISNULL(TransactionType,Side) TransactionType                      
  From T_MW_Transactions Transactions                  
  Order by TransactionType            
----------------------------------------------------------------------------            
-- IF NO DATA IN T_MW_Transactions SET UDASECTOR AS "No Data"            
----------------------------------------------------------------------------            
 If(Select Count(*) from #TransactionTypes) = 0            
 BEGIN            
  Insert into #TransactionTypes Values('No Data')            
 END            
             
 Select * from #TransactionTypes            
 Drop Table #TransactionTypes            
END 