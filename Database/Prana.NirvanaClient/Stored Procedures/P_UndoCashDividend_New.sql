/*            
--Author  : Rajat Tandon                                      
-- Date   : 04 May 2009                                      
-- Description : Undo the effect of supplied corporate actions.                                      
    
Modified By: Sandeep Singh    
Modified Date: Dec 16, 2013    
Desc: Cash Dividend is kept in table sum of dividend and group by Fund and Strategy.    
So on the basis of FKId and CorpActionID, we delete data from PM_CorpActionTaxlots and T_CashTransactions    
    
Modified BY Faisal Shah    
Modification Date Nov 17 2014    
Required to delete CashDividents on the basis of Taxlots which we sent if NAV was locked for some others    
    
*/    
CREATE Procedure [dbo].[P_UndoCashDividend_New]                                      
(                                      
 @CorpActionIDs varchar(max),     
 @TaxlotIds varchar(max),                                    
 @ErrorMessage varchar(500) output,                  
 @ErrorNumber int output                                              
)                                      
As                                      
                                     
SET @ErrorNumber = 0                                                                  
SET @ErrorMessage = 'Success'                                                
                                                          
BEGIN TRY                                                   
                                          
 BEGIN TRAN TRAN1                                             
    
select * into #TempCAIDs from dbo.Split(@CorpActionIDs,',') as Items    
select * into #TempTaxlotIds from dbo.Split(@taxlotIds,',') as TaxlotIds       
      
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
 Where CorpActionID In (Select Items From #TempCAIDs) And TaxlotId In (Select * From #TempTaxlotIds)       
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
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId              
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items               
  
----Faisal Shah           
--Delete                
--From T_CashTransactions  Where taxlotId In (Select * From #TempTaxlotIds)          
          
Delete T_CashTransactions           
From T_CashTransactions A     
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId          
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items   
Where A.TaxlotId In (Select * From #TempTaxlotIds)                   
              
----Faisal Shah              
--Delete               
--From PM_CorpActionTaxlots          
--Where taxlotId In (Select * From #TempTaxlotIds)

Delete PM_CorpActionTaxlots             
From PM_CorpActionTaxlots CATaxlots     
Inner join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items
Inner Join #TempTaxlotIds On #TempTaxlotIds.Items = CATaxlots.TaxlotId                 
              
Select * from #TempDividend       
            
Drop table #TempCAIDs,#Temp_PM_CorpActionTaxlots,#TempDividend,#TempTaxlotIds           
                             
COMMIT TRAN TRAN1                                            
                                                        
END TRY                                                                    
BEGIN CATCH                                                                     
 SET @ErrorMessage = ERROR_MESSAGE();                                                                    
 SET @ErrorNumber = Error_number();     
 ROLLBACK TRANSACTION TRAN1                                                                       
END CATCH;     

