  
/****************************************************************************  
Name :   PMAddUpdateExchangeMappings  
Date Created: 23-nov-2006   
Purpose:  Add Update Exchange Mappings  
Author: Ram Shankar Yadav  
Parameters:   
 @ThirdPartyID,  
 @Xml   
  
Execution Statement :   
 exec PMAddUpdateExchangeMappings 1, '<xml><element>value</element></xml>'  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE Proc [dbo].[PMAddUpdateExchangeMappings]  
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
UPDATE PM_DataSourceExchanges   
SET   
 PM_DataSourceExchanges.DataSourceExchangeName = XmlItem.SourceItemName,  
 PM_DataSourceExchanges.ApplicationExchangeID = XmlItem.ApplicationItemId  
   
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
 WITH   
  (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
 WHERE   
  PM_DataSourceExchanges.DataSourceExchangeID = XmlItem.SourceItemID AND XmlItem.ApplicationItemId > 0  
  
--This code inserts new data.  
  
Insert Into PM_DataSourceExchanges(ThirdPartyID, DataSourceExchangeName, ApplicationExchangeID)  
SELECT @ThirdPartyID, SourceItemName, ApplicationItemId  
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
  WITH   
   (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
Where XmlItem.SourceItemID Not IN (Select DataSourceExchangeID from PM_DataSourceExchanges)  AND XmlItem.ApplicationItemId > 0  
  
EXEC sp_xml_removedocument @handle  
  
COMMIT TRAN  
  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
  
END CATCH;  
  