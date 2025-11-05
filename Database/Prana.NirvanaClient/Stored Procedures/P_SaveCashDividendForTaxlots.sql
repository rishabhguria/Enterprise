
----------------------------------------------------                
--Modified By : Bharat Kumar Jangir                
--Modification Date :  07/08/2013                
--Description : Adding columns in Cash Dividend                
                
--Modified By : Narendra Kumar Jangir                
--Modification Date :  July 15 2013                
--Description : Renaming column Dividend to Amount for T_CashTransactions, Renaming table T_CashTransactions to T_CashTransactions                 
----------------------------------------------------                
----------------------------------------------------                  
--Modified By : Narendra Kumar Jangir                  
--Modification Date :  07/08/2013                  
--Description : Adding columns in Cash Divident                  
----------------------------------------------------                  
 ----------------------------------------------------                  
--Modified By : Narendra Kumar Jangir                  
--Modification Date :  07/08/2013                  
--Description : Adding columns in Cash Divident                  
----------------------------------------------------        
--Modified By : Nishant Kumar Jain              
--Modification Date :  2015-04-20             
--Description : Adding columns in TransactionSource,  EntryDate, ModifyDate   
-----------------------------------------------------            
                  
CREATE Procedure [dbo].[P_SaveCashDividendForTaxlots]                                                                      
(                                                                      
 @xml nText,                                                                      
 @ErrorMessage varchar(500) output,                                                                                                               
 @ErrorNumber int output                                                                                  
)                                                                      
                                                                      
As                  
                  
SET @ErrorNumber = 0                  
SET @ErrorMessage = 'Success'                  
                                                                       
                                                                      
BEGIN TRY                                                                                              
                  
BEGIN TRAN TRAN1                  
                  
DECLARE @handle int                  
                  
EXEC sp_xml_preparedocument @handle OUTPUT,@Xml                  
                                          
CREATE TABLE #TempTaxlots                                                                                                              
(                                      
 Level1ID int,                                                                          
 L2TaxlotID varchar(50),                                                                        
 CorpActionID uniqueidentifier,                                       
 Symbol varchar(50),                                                              
 Dividend float,                          
 CurrencyID int,                                                
 DivPayoutDate datetime,                                          
 ExDivDate datetime,                                        
 ParentTaxlot_PK bigint,                   
 TransactionIDs varchar(200),                  
 RecordDate datetime,                  
 DivDeclarationDate datetime,    
 Level2ID Int  ,  
 Description varchar(100)                                                                           
)                  
                  
INSERT INTO #TempTaxlots           
(          
 Level1ID,                  
 L2TaxlotID,                  
 CorpActionID,                  
 Symbol,                  
 Dividend,                  
 CurrencyID,                  
 DivPayoutDate,                  
 ExDivDate,                  
 ParentTaxlot_PK,                  
 TransactionIDs,                  
 RecordDate,                  
 DivDeclarationDate,    
Level2ID ,  
Description             
)                 
 SELECT                  
  Level1ID,                  
  L2TaxlotID,                  
  CorpActionID,                  
  Symbol,                  
  Dividend,                  
  CurrencyID,                  
  DivPayoutDate,                  
  ExDivDate,                  
  ParentTaxlot_PK,                  
  TransactionIDs,                  
  RecordDate,                  
  DivDeclarationDate ,    
  Level2ID,  
  Description                
 FROM OPENXML(@handle, '//TaxlotBase', 2)                  
 WITH                  
 (                  
 Level1ID int,                  
 L2TaxlotID varchar(50),                  
 CorpActionID uniqueidentifier,         
 Symbol varchar(50),                  
 Dividend float,                  
 CurrencyID int,                  
 DivPayoutDate varchar(50),                  
 ExDivDate varchar(50),                  
 ParentTaxlot_PK bigint,           
 TransactionIDs varchar(200),                  
 RecordDate varchar(50),                  
 DivDeclarationDate varchar(50),    
 Level2ID Int ,  
Description varchar(100)                        
 )                  
                  
INSERT INTO T_CashTransactions           
(          
 FundID,           
 TaxlotId,           
 Symbol,           
 Amount,           
 CurrencyID,           
 PayoutDate,           
 ExDate,           
 RecordDate,           
 DeclarationDate,          
 ActivityTypeId,    
 Level2Id,  
 TransactionSource,
 EntryDate,
 ModifyDate,  
 Description               
)                  
 SELECT                  
   Level1ID,                  
   L2TaxlotID,                  
   Symbol,                  
   Dividend,                  
   CurrencyID,                  
   DivPayoutDate,                  
   ExDivDate,                  
   RecordDate,                  
   DivDeclarationDate,            
  CASE           
  WHEN Dividend > 0           
  THEN (SELECT ActivityTypeId from T_ActivityType where ActivityType='DividendIncome')            
  ELSE (SELECT ActivityTypeId from T_ActivityType where ActivityType='DividendExpense')             
  END AS ActivityTypeId,    
Level2Id,  
'4', --Corporate Action    
GETDATE(),
 GETDATE(),  
 Description         
FROM #TempTaxlots                  
                  
INSERT INTO PM_CorpActionTaxlots           
(          
 CorpActionId,           
 FKId,           
 ParentRow_Pk,           
 TaxlotId,           
 L1AllocationID,           
 GroupId          
)                  
 SELECT                  
  Temp.CorpActionId,                  
  A.FK,                  
  Temp.ParentTaxlot_PK,                  
  Temp.L2TaxlotID,                  
--  --L2.Level1AllocationID,                  
  V.Level1AllocationID,          
  V.GroupId                  
 FROM #TempTaxlots Temp                  
 INNER JOIN (SELECT MAX(CashTransactionId) AS FK, TaxlotId FROM T_CashTransactions GROUP BY TaxlotId) A                  
 ON A.TaxlotId = temp.L2TaxlotID           
 INNER JOIN V_Taxlots V ON temp.L2TaxlotID = V.TaxlotId                  
---- INNER JOIN T_Level2Allocation L2 ON A.TaxlotId = L2.TaxLotID                  
 WHERE temp.CorpActionID IS NOT NULL AND temp.CorpActionID <> '00000000-0000-0000-0000-000000000000'                  
                  
                  
SELECT                  
 A.FK AS CashTransactionId,                  
 Temp.Level1ID AS FundID,                  
 Temp.L2TaxlotID AS TaxlotId,                  
 Temp.Symbol,                  
 Temp.Dividend as Amount,                  
 Temp.CurrencyID,                  
 Temp.DivPayoutDate AS PayoutDate,                  
 Temp.ExDivDate AS ExDate,            
 CASE           
 WHEN Dividend > 0           
 THEN (SELECT ActivityTypeId from T_ActivityType where ActivityType='DividendIncome')            
 ELSE (SELECT ActivityTypeId from T_ActivityType where ActivityType='DividendExpense')             
END AS ActivityTypeId      ,  
A.Description  
           
----INTO #TempDividend                  
FROM #TempTaxlots Temp                  
INNER JOIN (SELECT MAX(CashTransactionId) AS FK,max(Description) as Description,TaxlotId FROM T_CashTransactions GROUP BY TaxlotId) A                      
 ON A.TaxlotId = Temp.L2TaxlotID           
----INNER JOIN V_Taxlots V ON temp.L2TaxlotID = V.TaxlotId                  
----INNER JOIN T_Level2Allocation L2 ON A.TaxlotId = L2.TaxLotID                  
WHERE Temp.CorpActionID IS NOT NULL AND Temp.CorpActionID <> '00000000-0000-0000-0000-000000000000'                  
                  
--SELECT * FROM #TempDividend                  
                  
--DROP TABLE #TempDividend                  
DROP TABLE #TempTaxlots                  
                  
                  
EXEC sp_xml_removedocument @handle                  
                                                                          
COMMIT TRANSACTION TRAN1                  
                                                                                   
                                                              
END TRY                                                                                                
BEGIN CATCH                  
SET @ErrorMessage = ERROR_MESSAGE();                  
SET @ErrorNumber = ERROR_NUMBER();                  
                                                                                                 
 ROLLBACK TRANSACTION TRAN1                  
                                                         
END CATCH;   
  
