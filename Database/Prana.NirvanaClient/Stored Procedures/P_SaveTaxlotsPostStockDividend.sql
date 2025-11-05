 
    
CREATE Procedure [dbo].[P_SaveTaxlotsPostStockDividend]                                                      
(                                                      
 @xml XML,                                                      
 @ErrorMessage varchar(500) output,                                                                                               
 @ErrorNumber int output                                                                  
)                                                         
As         
                                                                         
--SET @ErrorNumber = 0                                                                          
--SET @ErrorMessage = 'Success'                                                       
                                                      
BEGIN TRY                                   
                                                      
 BEGIN TRAN TRAN1                                                                            
                                                                              
  DECLARE @handle int                                                                            
                                                                                                                           
  exec sp_xml_preparedocument @handle OUTPUT, @Xml                                
                          
  CREATE TABLE #TempTaxlots                                                                                                                    
  (                                                                  
 GroupID varchar(50),                                                       
 TaxlotID varchar(50),                                                        
 Symbol varchar(20),                                                        
 AvgPrice float,                                                        
 TaxlotOpenQty float,                                                        
 CorpActionID uniqueidentifier,                                                      
 PositionTag varchar(20),                                                      
 OrderSideTagValue varchar(20),                                                     
 AUECDate datetime,                                                      
 UTCDate datetime,                                                      
 TaxLotPK varchar(50),                                              
 ClosingTaxlotID varchar(50),                                 
 ParentTaxlot_PK bigint ,      
   OpenTotalCommissionandFees float,     
 FXRate FLoat,      
 FXConversionMethodOperator Varchar(5),      
 TradeAttribute1 Varchar(500),      
 TradeAttribute2 Varchar(500),      
 TradeAttribute3 Varchar(500),      
 TradeAttribute4 Varchar(500),      
 TradeAttribute5 Varchar(500),      
 TradeAttribute6 Varchar(500),      
 LotId Varchar(200),        
 ExternalTransId varchar(200),    
 CurrencyID Int,    
 DivPayoutDate varchar(50),                            
 ExDivDate varchar(50),                            
 RecordDate varchar(50),                            
 DivDeclarationDate varchar(50),    
 OldOpenQty Float,    
 OldAvgPrice Float,  
 Dividend Float                                           
  )                                                                                                                                            
                                                                                                                   
 Insert Into #TempTaxlots                                                                                                                                                                           
 (                                                                                                                                                                          
 GroupID,                                                        
 TaxlotID,                                    
 Symbol,                                                        
 AvgPrice,                                                        
 TaxlotOpenQty,                                                        
 CorpActionID ,                        
 PositionTag,                                                     
 OrderSideTagValue,                                                       
 AUECDate,                                                      
 UTCDate,                                                    
 TaxLotPK ,                                              
 ClosingTaxlotID,                            
 ParentTaxlot_PK,      
 OpenTotalCommissionandFees,    
 FXRate,      
 FXConversionMethodOperator ,      
 TradeAttribute1 ,      
 TradeAttribute2 ,      
 TradeAttribute3 ,      
 TradeAttribute4 ,      
 TradeAttribute5 ,      
 TradeAttribute6 ,      
 LotId,        
 ExternalTransId,    
 CurrencyID ,    
 DivPayoutDate,                            
 ExDivDate,                                         
 RecordDate,      
 DivDeclarationDate,    
 OldOpenQty,    
 OldAvgPrice,  
 Dividend                                    
  )                                                                               
  Select                                  
 GroupID,                            
 L2TaxlotID,                                                        
 Symbol,                    
 Cast(NewAvgPrice as float) as AvgPrice,                                                        
 Cast(NewTaxlotOpenQty as float) as TaxlotOpenQty,                            
 CorpActionID ,                                                      
 PositionTag,                        
 OrderSideTagValue,                                                       
 AUECDate,                                                      
 UTCDate,                                       
 TaxLotPK ,                                              
 ClosingTaxlotID,                            
 ParentTaxlot_PK ,      
 OpenTotalCommissionandFees,     
 FXRate,      
 FXConversionMethodOperator ,      
 TradeAttribute1 ,      
 TradeAttribute2 ,      
 TradeAttribute3 ,      
 TradeAttribute4 ,      
 TradeAttribute5 ,      
 TradeAttribute6 ,      
 LotId,        
 ExternalTransId,    
 CurrencyID,    
 DivPayoutDate,                            
 ExDivDate,                            
 RecordDate,                            
 DivDeclarationDate,    
 OpenQty,    
 AvgPrice,  
 Dividend                           
                                                              
  FROM OPENXML(@handle, '//TaxlotBase', 2)                                                                                                                           
  WITH                                                                                                
  (                                                        
 GroupID varchar(50),                                                        
 L2TaxlotID varchar(50),                                                        
 Symbol varchar(20),                            
 NewAvgPrice float,                                                        
 NewTaxlotOpenQty float,                                                
 CorpActionID uniqueidentifier,                                                      
 PositionTag varchar(20),                                                    
 OrderSideTagValue varchar(20),                               
 AUECDate varchar(50),                            
 UTCDate varchar(50),                                                      
 TaxLotPK varchar(50),                                        
 ClosingTaxlotID varchar(50),                            
 ParentTaxlot_PK bigint,      
 OpenTotalCommissionandFees float,     
 FXRate FLoat,      
 FXConversionMethodOperator Varchar(5),      
 TradeAttribute1 Varchar(500),      
 TradeAttribute2 Varchar(500),      
 TradeAttribute3 Varchar(500),      
 TradeAttribute4 Varchar(500),      
 TradeAttribute5 Varchar(500),      
 TradeAttribute6 Varchar(500),      
 LotId Varchar(200),        
 ExternalTransId varchar(200),    
 CurrencyID Int,    
 DivPayoutDate varchar(50),                            
 ExDivDate varchar(50),                            
 RecordDate varchar(50),                            
 DivDeclarationDate varchar(50),    
 OpenQty Float,    
 AvgPrice Float,  
 Dividend Float                                                             
  )                                                          
                        
Select       
 TempTaxlots.TaxLotID,      
 TempTaxlots.Symbol,      
 TempTaxlots.TaxLotOpenQty,                                                  
 TempTaxlots.AvgPrice,      
 TempTaxlots.UTCDate,      
 TempTaxlots.CorpActionID,      
 TempTaxlots.GroupID,                                                    
 TempTaxlots.AUECDate,      
 PM.FundID,      
 PM.Level2ID,       
 TempTaxlots.OpenTotalCommissionandFees,       
 PM.ClosedTotalCommissionandFees,       
 TempTaxlots.PositionTag,                     
 TempTaxlots.OrderSideTagValue ,       
 Cast(IsNull(TaxLotClosingId_Fk,'00000000-0000-0000-0000-000000000000') as UniqueIdentifier) as TaxLotClosingId_Fk,                    
 TempTaxlots.ParentTaxlot_PK,      
 TempTaxlots.FXRate,      
 TempTaxlots.FXConversionMethodOperator ,      
 TempTaxlots.TradeAttribute1 ,      
 TempTaxlots.TradeAttribute2 ,      
 TempTaxlots.TradeAttribute3 ,      
 TempTaxlots.TradeAttribute4 ,      
 TempTaxlots.TradeAttribute5 ,      
 TempTaxlots.TradeAttribute6 ,      
 TempTaxlots.LotId,        
 TempTaxlots.ExternalTransId,    
 TempTaxlots.CurrencyID,    
 TempTaxlots.DivPayoutDate,                            
 TempTaxlots.ExDivDate,                            
 TempTaxlots.RecordDate,                            
 TempTaxlots.DivDeclarationDate ,    
    TempTaxlots.OldOpenQty,    
 TempTaxlots.OldAvgPrice,  
 TempTaxlots.Dividend  
InTo #TempCATaxlots       
 From PM_TaxLots PM, #TempTaxlots TempTaxlots                                                  
  Where PM.TaxLot_PK = TempTaxlots.ParentTaxlot_PK    
       
Insert into PM_TaxLots                              
(      
  TaxLotID,      
  Symbol,      
  TaxLotOpenQty,      
  AvgPrice,      
  TimeOfSaveUTC,      
  GroupID,      
  AUECModifiedDate,      
  FundID,      
  Level2ID,                          
  OpenTotalCommissionandFees,      
  ClosedTotalCommissionandFees,      
  PositionTag,      
  OrderSideTagValue,      
  TaxLotClosingId_Fk,      
  ParentRow_Pk,      
  FXRate,      
  FXConversionMethodOperator ,      
  TradeAttribute1 ,      
  TradeAttribute2 ,      
  TradeAttribute3 ,      
  TradeAttribute4 ,      
  TradeAttribute5 ,      
  TradeAttribute6 ,      
  LotId,        
  ExternalTransId        
)                                                     
Select       
  TaxLotID,      
  Symbol,      
  TaxLotOpenQty,      
  AvgPrice,      
  UTCDate,      
  GroupID,      
  AUECDate,      
  FundID,      
  Level2ID,                          
  OpenTotalCommissionandFees,      
  ClosedTotalCommissionandFees,      
  PositionTag,      
  OrderSideTagValue,      
  TaxLotClosingId_Fk,      
  ParentTaxlot_PK,      
  FXRate,      
  FXConversionMethodOperator ,      
  TradeAttribute1 ,      
  TradeAttribute2 ,      
  TradeAttribute3 ,      
  TradeAttribute4 ,      
  TradeAttribute5 ,      
  TradeAttribute6 ,      
  LotId,        
  ExternalTransId             
    from #TempCATaxlots       
    
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
 Level2Id                    
)                            
 SELECT                            
 FundID,                            
 TaxLotID,                            
 Symbol,                            
------ (TaxLotOpenQty * AvgPrice) + ((OpenTotalCommissionandFees + ClosedTotalCommissionandFees) * dbo.GetSideMultiplier(OrderSideTagValue)) As Amount,          
--(TaxLotOpenQty * AvgPrice) + ((OpenTotalCommissionandFees) * dbo.GetSideMultiplier(OrderSideTagValue)) As Amount,                            
--((TaxLotOpenQty - OldOpenQty) * OldAvgPrice) + ((OpenTotalCommissionandFees) * dbo.GetSideMultiplier(OrderSideTagValue)) As Amount,                            
((Dividend - 1) * OldOpenQty* OldAvgPrice) As Amount,                            
 CurrencyID,                            
 DivPayoutDate,                            
 ExDivDate,                            
 RecordDate,                            
 DivDeclarationDate,       
 (SELECT ActivityTypeId from T_ActivityType where ActivityType='Stock Dividend') As ActivityTypeId,             
 Level2Id                       
FROM #TempCATaxlots                  
                    
Insert into PM_CorpActionTaxlots       
(      
 CorpActionId,       
 FKId,       
 ParentRow_Pk,       
 TaxlotId,       
 L1AllocationID,       
 GroupId      
)                      
Select       
Temp.CorpActionId,       
A.FK,       
Temp.ParentTaxlot_PK,       
Temp.TaxlotId,       
L2.Level1AllocationID ,       
Temp.GroupId       
 from #TempCATaxlots Temp Inner Join                    
 (Select Max(Taxlot_PK) as FK, TaxlotId from PM_taxlots group by TaxlotId) A on A.TaxlotId = Temp.TaxlotId     
 Inner Join T_Level2Allocation L2 on A.TaxlotId = L2.TaxLotID                  
 Where Temp.CorpActionID is not null and Temp.CorpActionID <> '00000000-0000-0000-0000-000000000000'    
    
SELECT                            
 T_CashTransactions.CashTransactionId AS CashTransactionId,                     
 Temp.FundID AS FundID,                            
 Temp.TaxlotId AS TaxlotId,                            
 Temp.Symbol,                            
 ---- (TaxLotOpenQty * AvgPrice) + ((OpenTotalCommissionandFees + ClosedTotalCommissionandFees) * dbo.GetSideMultiplier(OrderSideTagValue)) As Amount,                         
 ((TaxLotOpenQty - OldOpenQty) * OldAvgPrice) + ((OpenTotalCommissionandFees + ClosedTotalCommissionandFees) * dbo.GetSideMultiplier(OrderSideTagValue)) As Amount,                
  Temp.CurrencyID,                    
 Temp.DivPayoutDate AS PayoutDate,                            
 Temp.ExDivDate AS ExDate,                      
 (SELECT ActivityTypeId from T_ActivityType where ActivityType='Stock Dividend') As ActivityTypeId                       
Into #CashTransactions    
 FROM #TempCATaxlots Temp              
  Inner Join T_CashTransactions On T_CashTransactions.TaxlotID = Temp.TaxlotID And T_CashTransactions.FundID = Temp.FundID           
  And T_CashTransactions.Level2ID = Temp.Level2ID And T_CashTransactions.Symbol = Temp.Symbol          
  And T_CashTransactions.PayoutDate = Temp.DivPayoutDate And T_CashTransactions.ExDate = Temp.ExDivDate          
  And T_CashTransactions.RecordDate = Temp.RecordDate And T_CashTransactions.DeclarationDate = Temp.DivDeclarationDate                                      
  WHERE Temp.CorpActionID IS NOT NULL AND Temp.CorpActionID <> '00000000-0000-0000-0000-000000000000'                       
      
--Keep Cash Transaction entry into PM_CorpActionTaxlots table       
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
 #CashTransactions.CashTransactionId,                            
 Temp.ParentTaxlot_PK,                            
 Temp.TaxlotID,                                               
 L2.Level1AllocationID,                    
 Temp.GroupId                            
 FROM #TempCATaxlots Temp      
 Inner Join #CashTransactions On #CashTransactions.TaxlotID = Temp.TaxlotID And #CashTransactions.FundID = Temp.FundID    
 Inner Join T_Level2Allocation L2 ON Temp.TaxlotID = L2.TaxlotId                            
    
Select * from #CashTransactions    
                 
Drop Table #TempCATaxlots                      
Drop Table #TempTaxlots ,#CashTransactions                                                    
                                                  
EXEC sp_xml_removedocument @handle                                                                                
                                                                                 
COMMIT TRANSACTION TRAN1                                                                                
                                              
END TRY                                                                                
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                                                             
 SET @ErrorNumber = Error_number();                                                                                 
 ROLLBACK TRANSACTION TRAN1                                                                                   
END CATCH;         
        
      
