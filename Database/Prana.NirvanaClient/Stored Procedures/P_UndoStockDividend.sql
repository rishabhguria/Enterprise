
/*
--Author  : Sandeep Singh                        
-- Date   : 06 May 2014                             
-- Description : Undo the effect of supplied corporate actions. 
 
-- Modified By: Ankit Gupta on May 12, 2014
-- Description : Stop deleting data from PM_Taxlots and PM_CorpActionTaxlots for StockDIV
*/
CREATE Procedure [dbo].[P_UndoStockDividend]                              
(                              
	@corpactionIDs varchar(max),                            
	@ErrorMessage varchar(500) output,          
	@ErrorNumber int output                                      
)                              
As                              

--Declare @corpactionIDs varchar(max)                            
--Declare @ErrorMessage varchar(500)           
--Declare @ErrorNumber int      
--     
--Set @corpactionIDs = '0F8857BC-C176-495C-8AF4-0C360EF8A781'                  
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
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId            
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items             

Delete T_CashTransactions             
From T_CashTransactions A                
Inner Join #Temp_PM_CorpActionTaxlots CATaxlots on A.CashTransactionId = CATaxlots.FKId            
Inner Join #TempCAIDs Temp on CATaxlots.CorpActionID = Temp.Items 

Delete PM_Taxlots     
from PM_Taxlots A 
Inner Join PM_CorpActionTaxlots CATaxlots on A.TaxLot_PK = CATaxlots.FKId    
Inner Join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items        
Inner Join T_Group On T_Group.GroupID = A.GroupID     
Where T_Group.TransactionType <> 'StockDIV'             

Delete PM_CorpActionTaxlots       
from PM_CorpActionTaxlots CATaxlots 
Inner Join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items      
Inner Join T_Group On T_Group.GroupID = CATaxlots.GroupID     
Where T_Group.TransactionType <> 'StockDIV'           

Select * from #TempDividend    

Drop table #TempCAIDs,#TempDividend,#Temp_PM_CorpActionTaxlots      

COMMIT TRANSACTION TRAN1                                    

END TRY                                                            
BEGIN CATCH                                                             
SET @ErrorMessage = ERROR_MESSAGE();                                                            
SET @ErrorNumber = Error_number();                                                             
ROLLBACK TRANSACTION TRAN1                                                               
END CATCH;   
