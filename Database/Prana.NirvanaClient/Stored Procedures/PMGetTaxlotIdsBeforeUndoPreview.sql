
-- Author : Rajat                                          
-- Description : It provides the taxlotids for which corporate action is about to be removed.    
CREATE PROCEDURE [dbo].[PMGetTaxlotIdsBeforeUndoPreview]                                                
(                                                                                                                      
 @corpactionIDs varchar(max)                                                              
)                                                                                                                      
As                                                                                                                          
Begin                                                                                                          
-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                                                                                                      
Select distinct newTaxlots.CorpActionID, PT.TaxLotID as TaxLotID from PM_Taxlots PT Inner Join                                                                                                                    
(Select distinct CATaxlots.ParentRow_Pk, CATaxlots.CorpActionID from dbo.Split(@corpactionIDs,',') Splt            
Inner join PM_CorpActionTaxlots CATaxlots on Splt.Items = CATaxlots.CorpActionID) newTaxlots on newTaxlots.ParentRow_Pk = PT.Taxlot_PK             
                                                                                                  
End 

