CREATE PROCEDURE [dbo].[PMSaveFundCloseTradeData]                                                   
(                                                                        
   @Xml nText,                                                  
   @XmlTrade nText                                                                                                                                                                     
 , @ErrorMessage varchar(500) output                                                                                                                                                                      
 , @ErrorNumber int output                                                                                                                                  
)                                                                                                                                                   
AS                                                                     
BEGIN                                                                                                                                                                      
                                                                                                                                                                  
SET @ErrorMessage = 'Success'                                                                                                                                               
SET @ErrorNumber = 0                                                                                                                                                                      
                                                                                                                                                                
BEGIN TRAN TRAN1                                                                                                                                                                       
                                                                                                                                                      
BEGIN TRY                                                                                                                                                                     
                                                                                                                                                    
 DECLARE @handle1 int                                                                              
  --drop table    #TempTaxlots                                                                                                                        
  exec sp_xml_preparedocument @handle1 OUTPUT,@XmlTrade                                               
                                                          
                                                      
                                                  
 CREATE TABLE #TempTaxlots                                                                                                                          
 (                                                                  
 GroupID varchar(50),                                                        
 TaxLotID varchar(50) ,                                                        
 Symbol varchar(100),                                                        
 AvgPrice float,                                                        
 TaxLotQty float,                                                        
 PositionTag varchar(20),                                                      
 AUECModifiedDate datetime,                                                      
 TimeOfSaveUTC datetime,                                                      
 OpenTotalCommissionandFees float,                                                  
ClosedTotalCommissionandFees float,              
 FundID int,                                                  
 Level2ID int,                                                  
 OrderSideTagValue varchar(10),                                        
 TaxLotClosingId varchar(50),   
LotId varchar(200),  
ExternalTransId varchar(100)                                
 )                                                                                                                                                                           
Insert Into #TempTaxlots                                                                                                                 
(                                                                                     
 GroupID,                                      
 TaxLotID,                                                           
 Symbol,                                                        
 AvgPrice,                                                        
 TaxLotQty,                                                        
 PositionTag,                                                    
 AUECModifiedDate,                                                      
 TimeOfSaveUTC,                                          
 OpenTotalCommissionandFees ,                                                  
 ClosedTotalCommissionandFees ,                                                  
 FundID,                                                  
 Level2ID ,                                                  
 OrderSideTagValue  ,                                        
 TaxLotClosingId,  
LotId,  
ExternalTransId                                                 
  )                                                                                                                    
  Select                                                                                             
 GroupID,                                                        
 TaxLotID,                                                     
 Symbol,                                                        
 AvgPrice,                                                
 TaxLotQty,                                                        
 PositionTag,                                                    
 AUECModifiedDate,                                                      
 TimeOfSaveUTC,                                                    
 OpenTotalCommissionandFees,                                                  
 ClosedTotalCommissionandFees,                                                 
 Level1ID,                                                  
 Level2ID,                                                  
 OrderSideTagValue,                                        
 TaxLotClosingId,  
 LotId,  
 ExternalTransId                                                   
                                                     
                                                              
  FROM OPENXML(@handle1, '//TaxLot', 2)                                                                                                                                                                             
  WITH                                                                                                
  (                                                        
 GroupID varchar(50),                                                        
 TaxLotID varchar(50) ,                                                        
 Symbol varchar(100),                                                        
 AvgPrice float,                                                        
 TaxLotQty float,                                                        
 PositionTag varchar(20),                                                      
 AUECModifiedDate varchar(19),                                                      
 TimeOfSaveUTC  varchar(19),                                                      
 OpenTotalCommissionandFees float,                                                  
 ClosedTotalCommissionandFees float,                                                  
 Level1ID int,                                                  
 Level2ID int,                                                  
 OrderSideTagValue varchar(10),                                        
 TaxLotClosingId varchar(50),   
LotId varchar(200),  
ExternalTransId varchar(100)                                                 
                                                  
  )                                                 
 --drop table #TempTaxlots                         
                                         
                                                        
Insert into PM_TaxLots                              
(                            
TaxLotID,                      
 Symbol,                                                  
TaxLotOpenQty,                                                    
AvgPrice,                                                  
TimeOfSaveUTC ,                                                  
GroupID,                                                      
AUECModifiedDate,                                                  
FundID,                                                  
Level2ID,                                                  
OpenTotalCommissionandFees ,                                                  
ClosedTotalCommissionandFees,                                 
PositionTag,                                                  
OrderSideTagValue ,                                      
TaxLotClosingId_Fk,  
LotId,  
ExternalTransId                                 
)                                                       
 Select                                                  
tempTaxlots.TaxLotID,                                                  
 tempTaxlots.Symbol,                                                  
 tempTaxlots.TaxLotQty,                                                    
 tempTaxlots.AvgPrice,                                                  
tempTaxlots.TimeOfSaveUTC ,                                                  
 tempTaxlots.GroupID,                                                      
 tempTaxlots.AUECModifiedDate,                          
 tempTaxlots.FundID,                                                  
 tempTaxlots.Level2ID,                                                  
tempTaxlots.OpenTotalCommissionandFees,                                 
 tempTaxlots.ClosedTotalCommissionandFees,                                                    
 tempTaxlots.PositionTag,                                                  
 tempTaxlots.OrderSideTagValue,                                      
tempTaxlots.TaxLotClosingId,  
tempTaxlots.LotId,  
tempTaxlots.ExternalTransId                                                
                                      
                                                        
from  #TempTaxlots tempTaxlots                                                    
                                                
                                                 
                                                  
DECLARE @handle int                                                                            
EXEC sp_xml_preparedocument @Handle OUTPUT,@Xml                                                     
                                                                  
                                                     
                                                           
 CREATE TABLE #TempClosing                                                                                                                      
 (                                          
TaxLotClosingId varchar(50),                                                            
 PositionalTaxlotID varchar(50),                                                          
 ClosingTaxlotID varchar(50),                                                          
 ParentClosedQty float,                                                                
 IntClosingMode int,                                                          
 TimeOfSaveUTC datetime                                              
,Closedate  datetime                               
,PositionSide   varchar(10)              
,OpenPrice float              
,closePrice float              
,ClosingAlgo int                                       
 )                                                                                                                                            
                                                                                              
Insert Into #TempClosing                                                         
 (                                          
TaxLotClosingId ,                                                                                                                                                                    
 PositionalTaxlotID,                                                
 ClosingTaxlotID,                                                          
 ParentClosedQty,                                                                
 IntClosingMode,                                                          
 TimeOfSaveUTC                  
,Closedate                                
,PositionSide,              
OpenPrice,              
closePrice,      
ClosingAlgo              
                                      
 )                                                                                                                                           
 Select                                                                                                                        
TaxLotClosingId ,                                         
 ID,                                                          
 ClosingID,                                                      
 ClosedQty,                                                 
 IntClosingMode,                                                          
 TimeOfSaveUTC                                                           
,ClosingTradeDate                                  
,PositionSide,              
OpenAveragePrice,              
ClosedAveragePrice,      
ClosingAlgo              
                                                      
                                                    
                                                    
 FROM OPENXML(@handle, '//Position', 2)                                                                                                                                                                
 WITH                                                                                            
 (                                         
TaxLotClosingId varchar(50),                                                        
 ID varchar(50) ,                                                          
 ClosingID varchar(50),                                                          
 ClosedQty float,                                                                
 IntClosingMode int,                                                          
 TimeOfSaveUTC varchar(19)                                                          
,ClosingTradeDate  varchar(10)                                 
,PositionSide varchar(10)              
,OpenAveragePrice float,              
ClosedAveragePrice float,      
ClosingAlgo int                                                              
 )                                                 
                                                                       
 --Delete  PM_TaxlotClosing                                                
--drop table #tempTaxlots                                                                                                                 
                                    
  --select * from      PM_TaxlotClosing                                                              
                                                    
 Insert Into PM_TaxlotClosing                                                                                                
 (                                          
TaxLotClosingId ,                                                                                 
 PositionalTaxlotID,                                                          
 ClosingTaxlotID,          
 ClosedQty,                                                                
 ClosingMode,                                                          
 TimeOfSaveUTC,                                                 
 AuecLocalDate ,                                
PositionSide,              
OpenPrice,              
ClosePrice,      
 ClosingAlgo                   
                
 )                                                                                                
 Select                                          
 TaxLotClosingId ,                                                            
 PositionalTaxlotID,                                                          
 ClosingTaxlotID,                                                          
 ParentClosedQty,                                                                
 IntClosingMode,                     
 TimeOfSaveUTC,                                                          
 Closedate ,                                
 PositionSide,              
 OpenPrice,              
 closePrice,      
 ClosingAlgo                                                           
                                                            
 From #TempClosing                                      
                                  
--update PM_Taxlots                                  
--Set TaxLotClosingId_Fk = TaxLotClosingId                     
--, AUECModifiedDate = Closedate                                  
--from #TempClosing where ClosingTaxlotID=TaxlotID and #TempClosing.IntClosingMode in (2,4,5)                                  
                                                                         
 Drop Table #TempClosing,#tempTaxlots                                  
                                                                  
 EXEC sp_xml_removedocument @handle                                                            
 EXEC sp_xml_removedocument @handle1                                                             
                                                                                         
COMMIT TRANSACTION TRAN1                                                                                                        
                                                           
 END TRY                                                                                                                                                        
 BEGIN CATCH                                                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                                                                                                                                                      
 SET @ErrorNumber = Error_number();                                                           
 ROLLBACK TRANSACTION TRAN1                                                                                                                               
                                           
 END CATCH;                                                                                                    
                                                                                                                                                                      
END 

