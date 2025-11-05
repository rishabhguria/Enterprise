                                                         
CREATE PROCEDURE [dbo].[PMSaveSplitData]                                   
(                                                        
   @Xml nText,                                  
  @ErrorMessage varchar(500) output                                                                  
 , @ErrorNumber int output                                                                                                                  
)                                                                                                                                   
AS                                                     
BEGIN                                                                                                                                                      
                                                                                                                                                  
SET @ErrorMessage = 'Success'                                                                                                                               
SET @ErrorNumber = 0                                                                                                                                                      
                                                                                                                                                
                                                                                                                                      
                                                                                                                                                
BEGIN TRAN TRAN1                                                                                                                                                       
                                                                                                                                      
BEGIN TRY             
                                                                                                                                    
 DECLARE @handle int                                                              
 exec sp_xml_preparedocument @handle OUTPUT,@Xml                               
                                          
 CREATE TABLE #TempTaxlots                                                                                                          
 (                                                  
 GroupID varchar(50),                                        
 ID varchar(50) ,             
 Symbol varchar(20),                                        
 AveragePrice float,                                        
 TaxlotOpenQty float,                                        
 PositionTag varchar(20),                                      
 AUECLocalCloseDate datetime,                                      
 TimeOfSaveUTC datetime,                                      
 OpenTotalCommissionandFees float,                                  
 ClosedTotalCommissionandFees float,                                  
 FundID int,                                  
 StrategyID int,                                  
 SideID varchar(10),                        
TaxLotClosingId varchar(50),            
ParentRowPk bigint             
                                  
 )                                                                                                                            
                                                                                                   
  Insert Into #TempTaxlots                                                                                                                                                           
  (                                                                                                                                                          
 GroupID,                
 ID,                                           
 Symbol,                   
 AveragePrice,                                        
 TaxlotOpenQty,                                
 PositionTag,                                    
 AUECLocalCloseDate,                               
 TimeOfSaveUTC,                            
OpenTotalCommissionandFees ,              
ClosedTotalCommissionandFees ,                                  
 FundID,                                  
 StrategyID ,                    
SideID  ,                        
TaxLotClosingId  ,            
ParentRowPk                              
  )                                                                                                    
  Select                                                                             
 GroupID,                                        
 ID,                                     
 Symbol,                                        
 AveragePrice,                                
 OpenQty,                                        
 PositionTag,                                    
AUECLocalCloseDate ,                                      
TimeOfSaveUTC ,                                    
OpenTotalCommissionandFees ,                                  
 ClosedTotalCommissionandFees ,                                 
FundID,                                  
StrategyID ,                                  
SideID ,                        
TaxLotClosingId  ,            
ParentRowPk                              
                                     
FROM OPENXML(@handle, '//AllocatedTrade', 2)                                                                                                                                                             
  WITH                                                                                
  (                                        
 GroupID varchar(50),                                        
 ID varchar(50) ,                                        
 Symbol varchar(20),                                        
 AveragePrice float,                                        
 OpenQty float,                                        
 PositionTag varchar(20),                                      
 AUECLocalCloseDate varchar(19),                                      
 TimeOfSaveUTC  varchar(19),                                      
OpenTotalCommissionandFees float,                                  
 ClosedTotalCommissionandFees float,                                  
 FundID    int      'FundValue/FundID'     ,                                  
 StrategyID int  'StrategyValue/StrategyID' ,                                  
 SideID varchar(10),                        
 TaxLotClosingId varchar(50),            
ParentRowPk bigint                                   
                                  
  )                                 
 --drop table #TempTaxlots                          
                         
                                        
Insert into PM_TaxLots                                         
 Select                                  
 tempTaxlots.ID,                                  
 tempTaxlots.Symbol,                                  
 tempTaxlots.TaxLotOpenQty,                                    
 tempTaxlots.AveragePrice,                                  
tempTaxlots.TimeOfSaveUTC ,                                  
 null,                                  
 tempTaxlots.GroupID,                                      
 tempTaxlots.AUECLocalCloseDate,                                  
 tempTaxlots.FundID,                                  
 tempTaxlots.StrategyID,                                  
tempTaxlots.OpenTotalCommissionandFees ,                                  
 tempTaxlots.ClosedTotalCommissionandFees ,                                    
 tempTaxlots.PositionTag,                                  
 tempTaxlots.SideID ,                      
null  ,                     
tempTaxlots.ParentRowPk                        
                           
                                        
from  #TempTaxlots tempTaxlots                                    
        
         
      
                
                                                         
 Drop Table #tempTaxlots                  
                                                  
 EXEC sp_xml_removedocument @handle                                            
                                                                         
COMMIT TRANSACTION TRAN1                                                                                        
        
 END TRY                                                                                                                                                      
 BEGIN CATCH                                                 
 SET @ErrorMessage = ERROR_MESSAGE();                                                                          
 SET @ErrorNumber = Error_number();                                           
 ROLLBACK TRANSACTION TRAN1                         
                                           
 END CATCH;                                                                                    
                                                                                                                                                      
END   
  
  
  