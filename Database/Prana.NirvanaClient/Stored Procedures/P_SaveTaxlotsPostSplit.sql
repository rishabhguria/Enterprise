
/*
Sandeep Singh:
Modifed Date: 06 DEC 2014
Desc: Set TaxLotClosingId_Fk to null
TaxLotClosingId_Fk should be null while applying the split Corporate action. 
If a trade is open i.e. partially not closed, then TaxLotClosingId_Fk will be null. 
Assume a trade is partially closed, then TaxLotClosingId_Fk will have some value. 
Same value is copied on next row i.e. which is entered with split CA. Now when we fetch data on the basis of TaxLotClosingId_Fk, 
more than one record will be fetched. This is happened on JCAP release and data duplicates on Realized and MTM reports. 
So I set TaxLotClosingId_Fkï¿½s value to null in case of Split CA.
http://jira.nirvanasolutions.com:8080/browse/PRANA-5556
*/
      
CREATE Procedure [dbo].[P_SaveTaxlotsPostSplit]                                                      
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
                                                                                                                           
  exec sp_xml_preparedocument @handle OUTPUT, @Xml                                
                                        
---- drop table #TempTaxlots                            
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
	ClosedTotalCommissionandFees float,    
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
	AdditionalTradeAttributes varchar(max)
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
	ClosedTotalCommissionandFees,    
	FXRate,      
	FXConversionMethodOperator ,      
	TradeAttribute1 ,      
	TradeAttribute2 ,      
	TradeAttribute3 ,      
	TradeAttribute4 ,      
	TradeAttribute5 ,      
	TradeAttribute6 ,      
	LotId,        
	ExternalTransId ,
	AdditionalTradeAttributes
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
	ClosedTotalCommissionandFees,     
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
	AdditionalTradeAttributes
              
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
	ClosedTotalCommissionandFees float,    
	FXRate FLoat,      
	FXConversionMethodOperator Varchar(5),      
	TradeAttribute1 Varchar(500),      
	TradeAttribute2 Varchar(500),      
	TradeAttribute3 Varchar(500),      
	TradeAttribute4 Varchar(500),      
	TradeAttribute5 Varchar(500),      
	TradeAttribute6 Varchar(500),      
	LotId Varchar(200),        
	ExternalTransId varchar(200) ,
	AdditionalTradeAttributes varchar(max)
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
	TempTaxlots.ClosedTotalCommissionandFees,       
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
	TempTaxlots.AdditionalTradeAttributes
InTo #TempCATaxlots       
From PM_TaxLots PM, #TempTaxlots tempTaxlots                                                  
Where PM.TaxLot_PK = tempTaxlots.ParentTaxlot_PK                                                 

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
	ExternalTransId ,
	AdditionalTradeAttributes
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
	Null,--TaxLotClosingId_Fk,      
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
	ExternalTransId,
	AdditionalTradeAttributes
from #TempCATaxlots                    

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
temp.CorpActionId,       
A.FK,       
temp.ParentTaxlot_PK,       
temp.TaxlotId,       
L2.Level1AllocationID ,       
temp.GroupId       
from #TempCATaxlots temp inner join                    
  (Select max(Taxlot_PK) as FK, TaxlotId from PM_taxlots group by TaxlotId) A on A.TaxlotId = temp.TaxlotId inner join                   
  T_Level2Allocation L2 on A.TaxlotId = L2.TaxLotID                  
  where temp.CorpActionID is not null and temp.CorpActionID <> '00000000-0000-0000-0000-000000000000'                      
                      
Drop Table #TempCATaxlots                      
Drop Table #TempTaxlots                                                      
                                                  
EXEC sp_xml_removedocument @handle                                                                                
                                                                                 
COMMIT TRANSACTION TRAN1                                                                                
                                              
END TRY                                                                                
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                                                             
 SET @ErrorNumber = Error_number();                                                                                 
 ROLLBACK TRANSACTION TRAN1                                                                                   
END CATCH;         

