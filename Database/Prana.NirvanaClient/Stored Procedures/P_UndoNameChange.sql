CREATE Procedure [dbo].[P_UndoNameChange]                          
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
  
 Delete T_Level2Allocation   
 from T_Level2Allocation A inner join PM_CorpActionTaxlots CATaxlots on A.TaxlotID = CATaxlots.TaxlotID  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
  
 Delete T_FundAllocation   
 from T_FundAllocation A inner join PM_CorpActionTaxlots CATaxlots on A.AllocationId = CATaxlots.L1AllocationID  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
   
 -- Delete Newly created Addition/Withdrawals  
 Delete PM_Taxlots   
 from PM_Taxlots A inner join PM_CorpActionTaxlots CATaxlots on A.TaxLot_PK = CATaxlots.FKId  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
   
 -- Delete the closing of Withdrawal with original transaction  
 Delete PM_Taxlots   
 from PM_Taxlots A inner join PM_CorpActionTaxlots CATaxlots on A.TaxLotClosingId_Fk = CATaxlots.ClosingId  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
  
 Delete PM_TaxlotClosing   
 from PM_TaxlotClosing A inner join PM_CorpActionTaxlots CATaxlots on A.TaxLotClosingID = CATaxlots.ClosingId  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
       
 Delete T_Group   
 from T_Group A inner join PM_CorpActionTaxlots CATaxlots on A.GroupID = CATaxlots.GroupID  
 inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
  
 Delete PM_CorpActionTaxlots   
 from PM_CorpActionTaxlots CATaxlots inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items  
  
 Drop table #TempCAIDs  
                   
COMMIT TRANSACTION TRAN1                                
                                            
END TRY                                                        
BEGIN CATCH                                                         
 SET @ErrorMessage = ERROR_MESSAGE();                                                        
 SET @ErrorNumber = Error_number();                                                         
 ROLLBACK TRANSACTION TRAN1                                                           
END CATCH; 

