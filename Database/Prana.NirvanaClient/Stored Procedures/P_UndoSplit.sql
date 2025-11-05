
/*      
--Author  : Rajat Tandon                                  
-- Date   : 04 May 2009                                  
-- Description : Undo the effect of supplied corporate actions.     
  
-- Modified By: Ankit Gupta on May 12, 2014  
-- Description : Stop deleting data from PM_Taxlots and PM_CorpActionTaxlots for StockSPLIT  
*/                               
CREATE Procedure [dbo].[P_UndoSplit]                                  
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
        
 Delete PM_Taxlots         
 from PM_Taxlots A inner join PM_CorpActionTaxlots CATaxlots on A.TaxLot_PK = CATaxlots.FKId        
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items     
Inner Join T_Group On T_Group.GroupID = A.GroupID     
Where T_Group.TransactionSource <> 10 And (T_Group.TransactionType <> 'ShortWithdrawalCashInLieu' And T_Group.TransactionType <> 'LongWithdrawalCashInLieu')    
        
 Delete PM_CorpActionTaxlots           
 from PM_CorpActionTaxlots CATaxlots inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items       
Inner Join T_Group On T_Group.GroupID = CATaxlots.GroupID     
Where T_Group.TransactionSource <> 10 And (T_Group.TransactionType <> 'ShortWithdrawalCashInLieu' And T_Group.TransactionType <> 'LongWithdrawalCashInLieu')
        
 Drop table #TempCAIDs          
        
 COMMIT TRANSACTION TRAN1                                        
                                                    
END TRY                                                                
BEGIN CATCH                                                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                                                
 SET @ErrorNumber = Error_number();                                                                 
 ROLLBACK TRANSACTION TRAN1                                                                   
END CATCH;          

