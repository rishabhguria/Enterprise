
                           
/****************************************************************************                              
Name :   [PMSaveSimulatedPositions]                              
Date Created: 18-Mar-2007                               
Purpose:  Saves simulated positions                             
Author: Bhupesh Bareja                              
Parameters:                               
   @Xml nText                              
 , @ErrorMessage varchar(500) output                              
 , @ErrorNumber int output                         
                               
                              
Execution Statement :                               
	Exec PMSaveSimulatedPositions '<?xml version="1.0" ?> 
 <ArrayOfOTCPosition xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <OTCPosition>
  <BrokenRulesCollection /> 
  <ID>00000000-0000-0000-0000-000000000000</ID> 
  <StartTaxLotID>0</StartTaxLotID> 
  <ModifiedAt>0001-01-01T00:00:00</ModifiedAt> 
  <StartDate>0001-01-01T00:00:00</StartDate> 
  <ModifiedBy>0</ModifiedBy> 
  <Symbol>MSFT</Symbol> 
  <FundID>0</FundID> 
 <FundValue>
  <BrokenRulesCollection /> 
  <ID>0</ID> 
  <ShortName /> 
  <FullName /> 
  </FundValue>
  <AveragePrice>23</AveragePrice> 
  <PositionType>1</PositionType> 
  <AUECID>1</AUECID> 
  <OpenQty>0</OpenQty> 
  <Multiplier>1</Multiplier> 
  <ClosedQty>2421</ClosedQty> 
  <PositionStartQuantity>2421</PositionStartQuantity> 
  <IsActive>true</IsActive> 
  <PNL>0</PNL> 
  <Commission>0</Commission> 
  <Fees>0</Fees> 
  <PositionTaxLots /> 
  <ToBeIncluded>true</ToBeIncluded> 
  <AssetID>0</AssetID> 
  <UnderLyingID>0</UnderLyingID> 
  <VenueID>0</VenueID> 
  <SymbolConventionID>0</SymbolConventionID> 
  <StrategyID>0</StrategyID> 
  <CounterPartyID>0</CounterPartyID> 
  </OTCPosition>
  </ArrayOfOTCPosition>',  '', 0                          
          
                              
select * from PM_SimulatedPositions       
      
                              
Date Modified: <>                              
Description:   <>                              
Modified By:   <>                              
****************************************************************************/                              
CREATE PROCEDURE [dbo].[PMSaveSimulatedPositions]                              
                              
   @Xml nText                                             
 , @ErrorMessage varchar(500) output                              
 , @ErrorNumber int output                              
                              
AS                               
BEGIN                              
                          
SET @ErrorMessage = 'Success'                              
SET @ErrorNumber = 0                              
                              
--BEGIN TRAN TRAN1                               
                              
BEGIN TRY           
                          
                              
DECLARE @handle int                                 
exec sp_xml_preparedocument @handle OUTPUT,@Xml                                 
                         
  /*                  
CREATE TABLE #TempSimulatedPositions                               
  (                               
	 Symbol varchar(5)           
   , SideID int                              
   , Quantity int                               
   , AveragePrice float                
   , AUECID int          
   --, IsActive bit          
       
  )       
*/                         
INSERT INTO                               
 PM_SimulatedPositions    
  (                              
	 Symbol    
   , SideID    
   , Quantity    
   , AveragePrice    
   , AuecID    
   --, IsActive    
  )                          
                         
Select                               
	  Symbol                              
  , PositionType                               
  , PositionStartQuantity                              
  , AveragePrice                              
  , AUECID                               
  --, IsActive          
      
FROM OPENXML(@handle, '//OTCPosition', 2)                                 
 WITH                               
 (                              
	  Symbol varchar(5)           
   , PositionType int                              
   , PositionStartQuantity int                               
   , AveragePrice float                
   , AUECID int          
   --, IsActive bit        
           
 )                               
                        
                 
--This code inserts new data.                             
             
   /*                           
SELECT                               
	temp.Symbol                               
  , temp.SideID                  
  , temp.Quantity                  
  , Temp.AveragePrice                              
  , Temp.AUECID                              
  --, Temp.IsActive               
                               
FROM                              
 #TempSimulatedPositions as temp                               
 --LEFT OUTER JOIN T_CompanyAUEC AS TCA ON Temp.AuecID = TCA.AuecId          
 --LEFT OUTER JOIN T_Underlying AS TU ON Temp.Underlying = TU.UnderlyingName           
          
 drop table #TempSimulatedPositions                              
*/
EXEC sp_xml_removedocument @handle                              
                                                     
                              
END TRY                              
BEGIN CATCH                               
 SET @ErrorMessage = ERROR_MESSAGE();                              
 SET @ErrorNumber = Error_number();                       
         
END CATCH;                              
                    
                  
END 
