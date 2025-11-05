
/*        
--Author  : Rajat Tandon                                  
-- Date   : 04 May 2009                                  
-- Description : Undo the effect of supplied corporate actions.                                  

Modified By: Sandeep Singh
Modified Date: Dec 16, 2013
Desc: Cash Dividend is kept in table sum of dividend and group by Fund and Strategy.
So on the basis of FKId and CorpActionID, we delete data from PM_CorpActionTaxlots and T_CashTransactions

*/
CREATE Procedure [dbo].[P_UndoCashDividend]                                  
(                                  
 @corpactionIDs varchar(max),                                
 @ErrorMessage varchar(500) output,              
 @ErrorNumber int output                                          
)                                  
As                                  
                                  
SET @ErrorNumber = 0                                                              
SET @ErrorMessage = 'Success'                                            
                                                      
BEGIN TRY                                               
                                      
 BEGIN TRAN TRAN1                                         
               
 Select * into #TempCAIDs from dbo.Split(@corpactionIDs,',') as Items       
  
-- keep FKId and CorpActionID And delete on the basis of these 2 fields
Create Table #Temp_PM_CorpActionTaxlots    
(  
 FKId Int,  
 CorpActionID UniqueIdentifier  
)    
Insert Into #Temp_PM_CorpActionTaxlots  
Select   
 FKId,  
 CorpActionID  
 From PM_CorpActionTaxlots  
 Where CorpActionID In (Select Items From #TempCAIDs)   
 Group By FKId,CorpActionID  
       
        
Select     
 A.CashTransactionId,    
 A.FundID,     
 A.TaxlotId,     
 A.Symbol,     
 A.Amount,     
 A.CurrencyID,     
 A.PayoutDate,     
 A.ExDate,     
 A.ActivityTypeId       
InTo #TempDividend          
From T_CashTransactions A     
--Inner Join PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId          
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId          
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items           
        
Delete T_CashTransactions           
From T_CashTransactions A     
--Inner Join PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId          
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId          
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items         
        
--Delete T_CashDivTransactions          
--from  T_CashDivTransactions B inner join PM_CorpActionTaxlots CATaxlots on B.FKId = CATaxlots.FKId          
-- inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items               
          
        
Delete PM_CorpActionTaxlots             
From PM_CorpActionTaxlots CATaxlots     
Inner join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items          
          
Select * from #TempDividend   
        
Drop table #TempCAIDs,#Temp_PM_CorpActionTaxlots,#TempDividend        
                         
COMMIT TRANSACTION TRAN1                                        
                                                    
END TRY                                                                
BEGIN CATCH                                                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                                                
 SET @ErrorNumber = Error_number();                                                                 
 ROLLBACK TRANSACTION TRAN1                                                                   
END CATCH;           

