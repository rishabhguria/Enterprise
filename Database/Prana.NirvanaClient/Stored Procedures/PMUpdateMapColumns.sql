/****************************************************************************        
Name :   PMUpdateMapColumns        
Date Created: 27-nov-2006         
Purpose:  Update Columns Mappings        
Author:   Ram Shankar Yadav        
Parameters:         
 @Xml nText,        
 @ErrorNumber int output,        
 @ErrorMessage varchar(200) output        
        
Execution Statement :         
 exec [PMUpdateMapColumns] 1, 2, '<?xml version="1.0"?>    
<ArrayOfMappingItem xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>1</SourceItemID>    
    <SourceItemName>Quantity</SourceItemName>    
    <ApplicationItemId>4</ApplicationItemId>    
    <ApplicationItemName>NetPosition</ApplicationItemName>    
    <Lock>true</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>2</SourceItemID>    
    <SourceItemName>SecSymbol</SourceItemName>    
    <ApplicationItemId>1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>true</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>3</SourceItemID>    
    <SourceItemName>TaxLotDesc</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName>TradeCurrency</ApplicationItemName>    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>4</SourceItemID>    
    <SourceItemName>TaxLotID</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>5</SourceItemID>    
    <SourceItemName>LotDate</SourceItemName>    
    <ApplicationItemId>15</ApplicationItemId>    
    <ApplicationItemName>PositionStartDate</ApplicationItemName>    
    <Lock>true</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>6</SourceItemID>    
    <SourceItemName>UnitCostLocal</SourceItemName>    
    <ApplicationItemId>3</ApplicationItemId>    
    <ApplicationItemName>CostBasis</ApplicationItemName>    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>7</SourceItemID>    
    <SourceItemName>MarketPriceLocal</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>8</SourceItemID>    
    <SourceItemName>TotalCostLocal</SourceItemName>    
    <ApplicationItemId>28</ApplicationItemId>    
    <ApplicationItemName>UnderlyingName</ApplicationItemName>    
    <Lock>true</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>9</SourceItemID>    
    <SourceItemName>MarketValueLocal</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>10</SourceItemID>    
    <SourceItemName>TotalCostBook</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>11</SourceItemID>    
    <SourceItemName>MarketValueBook</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>12</SourceItemID>    
    <SourceItemName>AccruedInterest</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>13</SourceItemID>    
    <SourceItemName>PriceGainLoss</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>14</SourceItemID>    
    <SourceItemName>FXGain_Loss</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>15</SourceItemID>    
    <SourceItemName>UnrealizedGain_Loss</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection />    
    <SourceItemID>138</SourceItemID>    
    <SourceItemName>FundName</SourceItemName>    
    <ApplicationItemId>12</ApplicationItemId>    
    <ApplicationItemName>FundName</ApplicationItemName>    
    <Lock>true</Lock>    
  </MappingItem>    
  <MappingItem>    
    <BrokenRulesCollection>    
      <BrokenRule />    
    </BrokenRulesCollection>    
    <SourceItemID>139</SourceItemID>    
    <SourceItemName>UploadID</SourceItemName>    
    <ApplicationItemId>-1</ApplicationItemId>    
    <ApplicationItemName />    
    <Lock>false</Lock>    
  </MappingItem>    
</ArrayOfMappingItem>', 0, ''        
        
select * from dbo.PM_DataSourceColumns where ThirdPartyID = 1 and tabletypeid = 2      
      
Date Modified: <DateModified>         
Description:     <DescriptionOfChange>         
Modified By:     <ModifiedBy>         
****************************************************************************/        
Create Proc [dbo].[PMUpdateMapColumns]        
(        
  @ThirdPartyID int,       
  @TableTypeID int,      
  @Xml nText,        
  @ErrorNumber int output,        
  @ErrorMessage varchar(200) output        
 )        
AS         
        
SET @ErrorNumber = 0        
SET @ErrorMessage = 'Success'        
        
BEGIN TRY        
        
BEGIN TRAN        
        
DECLARE @handle int           
exec sp_xml_preparedocument @handle OUTPUT,@Xml           
  /*    
UPDATE         
 PM_DataSourceColumns      
SET       
 ApplicationColumnId = null      
WHERE       
 ThirdPartyID = @ThirdPartyID       
 AND      
 TableTypeID = @TableTypeID      
   */    
--This code updates old data.        
UPDATE PM_DataSourceColumns         
SET         
 PM_DataSourceColumns.ApplicationColumnId = XmlItem.ApplicationItemId,        
 PM_DataSourceColumns.Locked = XmlItem.Lock        
         
FROM         
 OPENXML(@handle, '//MappingItem', 2)           
 WITH         
  (SourceItemID Integer, ApplicationItemId Integer, Lock bit)  XmlItem        
 WHERE         
  PM_DataSourceColumns.DataSourceColumnID = XmlItem.SourceItemID       
  AND     
  XmlItem.ApplicationItemId <> -1        
  AND     
  TableTypeID = @TableTypeID     
     
    UPDATE PM_DataSourceColumns         
SET         
 PM_DataSourceColumns.ApplicationColumnId = NULL   ,        
 PM_DataSourceColumns.Locked = XmlItem.Lock               
FROM         
 OPENXML(@handle, '//MappingItem', 2)           
 WITH         
  (SourceItemID Integer, ApplicationItemId Integer, Lock bit)  XmlItem        
 WHERE         
  PM_DataSourceColumns.DataSourceColumnID = XmlItem.SourceItemID       
  AND XmlItem.ApplicationItemId = -1      
  AND    
 TableTypeID = @TableTypeID    
        
EXEC sp_xml_removedocument @handle        
        
        
COMMIT TRAN        
        
END TRY        
BEGIN CATCH        
         
 SET @ErrorNumber = ERROR_NUMBER();        
 SET @ErrorMessage = ERROR_MESSAGE();        
         
 ROLLBACK TRAN        
        
END CATCH; 