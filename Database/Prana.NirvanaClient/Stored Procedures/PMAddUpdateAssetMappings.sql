  
/****************************************************************************  
Name :   PMAddUpdateAssetMappings  
Date Created: 23-nov-2006   
Purpose:  Add Update AssetMappings Columns  
Author: Ram Shankar Yadav  
Parameters:   
 @ThirdPartyID int,  
 @Xml nText,  
 @ErrorNumber int output,  
 @ErrorMessage varchar(200) output   
  
Execution Statement :   
 exec PMAddUpdateAssetMappings 1, '<xml><element>value</element></xml>'  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE Proc [dbo].[PMAddUpdateAssetMappings]  
(  
 @ThirdPartyID int,  
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
  
--This code updates old data.  
UPDATE PM_DataSourceAssets   
SET   
 PM_DataSourceAssets.DataSourceAssetName = XmlItem.SourceItemName,  
 PM_DataSourceAssets.ApplicationAssetID = XmlItem.ApplicationItemId  
   
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
 WITH   
  (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
 WHERE   
  PM_DataSourceAssets.DataSourceAssetID = XmlItem.SourceItemID AND XmlItem.ApplicationItemId > 0  
  
--This code inserts new data.  
  
Insert Into PM_DataSourceAssets(ThirdPartyID, DataSourceAssetName, ApplicationAssetID)  
SELECT @ThirdPartyID, SourceItemName, ApplicationItemId  
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
  WITH   
   (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
Where XmlItem.SourceItemID Not IN (Select DataSourceAssetID from PM_DataSourceAssets) AND XmlItem.ApplicationItemId > 0  
  
EXEC sp_xml_removedocument @handle  
  
COMMIT TRAN  
  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
  
END CATCH;  
  
  
