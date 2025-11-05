   
  
/****************************************************************************  
Name :   PMAddUpdateCurrencyMappings  
Date Created: 23-nov-2006   
Purpose:  Add Update Currency Mappings  
Author: Ram Shankar Yadav  
Parameters:   
 @ThirdPartyID int,  
 @Xml nText,  
 @ErrorNumber int output,  
 @ErrorMessage varchar(200) output  
  
Execution Statement :   
 exec PMAddUpdateCurrencyMappings 1, '<xml><element>value</element></xml>'  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE Proc [dbo].[PMAddUpdateCurrencyMappings]  
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
UPDATE PM_DataSourceCurrencies   
SET   
 PM_DataSourceCurrencies.DataSourceCurrencyName = XmlItem.SourceItemName,  
 PM_DataSourceCurrencies.ApplicationCurrencyID = XmlItem.ApplicationItemId  
   
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
 WITH   
  (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
 WHERE   
  PM_DataSourceCurrencies.DataSourceCurrencyID = XmlItem.SourceItemID AND XmlItem.ApplicationItemId > 0  
  
--This code inserts new data.  
  
Insert Into PM_DataSourceCurrencies(ThirdPartyID, DataSourceCurrencyName, ApplicationCurrencyID)  
SELECT @ThirdPartyID, SourceItemName, ApplicationItemId  
FROM   
 OPENXML(@handle, '//MappingItem', 2)     
  WITH   
   (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem  
Where XmlItem.SourceItemID Not IN (Select DataSourceCurrencyID from PM_DataSourceCurrencies)  AND XmlItem.ApplicationItemId > 0  
  
EXEC sp_xml_removedocument @handle  
  
COMMIT TRAN  
  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
  
END CATCH;  
  
