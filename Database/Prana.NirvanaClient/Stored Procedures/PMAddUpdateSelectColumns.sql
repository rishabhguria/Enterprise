/****************************************************************************  
Name :   PMAddUpdateSelectColumns  
Date Created: 17-nov-2006   
Purpose:  Add Update DataSource Columns  
Author: Ram Shankar Yadav  
Parameters:   
 @ThirdPartyID int,  
 @Xml nText,  
 @ErrorNumber int output,  
 @ErrorMessage varchar(200) output   
  
Execution Statement :   
 exec PMAddUpdateSelectColumns 1, '<xml><element>value</element></xml>'  
  
Date Modified: 24 Nov 2006  
Description:    Added a new field 'ColumnSequenceNo','RequiredInUpload'  
Modified By:     Rajat    
****************************************************************************/  
CREATE Proc [dbo].[PMAddUpdateSelectColumns]  
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
  
DECLARE @hDoc int     
exec sp_xml_preparedocument @hDoc OUTPUT,@Xml     
  
--This code updates old data.  
UPDATE PM_DataSourceColumns   
SET   
 PM_DataSourceColumns.ColumnName = XmlItem.SourceColumnName,  
 PM_DataSourceColumns.Description = XmlItem.Description,  
 PM_DataSourceColumns.Type = XmlItem.Type,  
 PM_DataSourceColumns.SampleValue = XmlItem.SampleValue,  
 PM_DataSourceColumns.Notes = XmlItem.Notes,  
 PM_DataSourceColumns.ColumnSequenceNo = XmlItem.ColumnSequenceNo,  
 PM_DataSourceColumns.RequiredInUpload = XmlItem.IsRequiredInUpload,  
 PM_DataSourceColumns.TableTypeID = XmlItem.TableTypeID  
FROM   
 OPENXML(@hDoc, '//SelectColumnsItem', 2)     
 WITH   
  (ID Integer, SourceColumnName nvarchar(50), Description nvarchar(500),Type tinyint,  SampleValue nvarchar(100),  Notes nvarchar(500),ColumnSequenceNo Integer, IsRequiredInUpload bit, TableTypeID Integer)  XmlItem  
 WHERE   
  PM_DataSourceColumns.DataSourceColumnID = XmlItem.ID  
  
--This code inserts new data.  
  
Insert Into PM_DataSourceColumns(ThirdPartyID, ColumnName, Description, Type, SampleValue, Notes,ColumnSequenceNo, RequiredInUpload, TableTypeID)  
SELECT @ThirdPartyID, SourceColumnName, Description, Type, SampleValue, Notes,ColumnSequenceNo, IsRequiredInUpload, TableTypeID  
FROM       OPENXML (@hdoc, '//SelectColumnsItem',2)  
WITH (ID Integer, SourceColumnName nvarchar(50), Description nvarchar(500),Type tinyint,  SampleValue nvarchar(100),  Notes nvarchar(500),ColumnSequenceNo Integer, IsRequiredInUpload bit, TableTypeID Integer)  XmlItem  
Where XmlItem.ID Not IN (Select DataSourceColumnID from PM_DataSourceColumns)  
AND XmlItem.SourceColumnName != ''  
  
EXEC sp_xml_removedocument @hDoc  
  
COMMIT TRAN  
  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
  
END CATCH;  
  