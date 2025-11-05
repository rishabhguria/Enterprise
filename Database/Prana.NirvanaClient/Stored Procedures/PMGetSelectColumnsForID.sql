     
/****************************************************************************      
Name :   [PMGetSelectColumnsForID]      
Date Created: 23-nov-2006       
Purpose:  Get all the Select Columns for specified company      
Author: Ram Shankar Yadav      
Execution Statement : exec [PMGetSelectColumnsForID] 1 , '', 0     
      
Date Modified: 24 Nov 2006      
Description:     Added ColumnSequenceNo, RequiredInUpload      
Modified By:     Rajat      
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetSelectColumnsForID]      
(      
  @ThirdPartyID int,      
  @ErrorMessage varchar(500) output,      
  @ErrorNumber int output      
)      
AS      
SET @ErrorMessage = 'Success'      
SET @ErrorNumber = 0      
BEGIN TRY      
 SELECT       
  DataSourceColumnID,      
  ColumnName,      
  ISNULL(Description, '') AS Description,      
  Type,      
  ISNULL(SampleValue, '') AS SampleValue,      
  ISNULL(Notes,'') AS Notes,      
  ISNULL(ColumnSequenceNo,'') AS ColumnSequenceNo,      
  ISNULL(RequiredInUpload,'') AS RequiredInUpload          
 FROM       
  PM_DataSourceColumns      
 WHERE      
  ThirdPartyID = @ThirdPartyID      
 AND  
 ColumnName <> 'UploadID'  
      
END TRY      
BEGIN CATCH       
 SET @ErrorMessage = ERROR_MESSAGE();      
 SET @ErrorNumber = Error_number();       
END CATCH; 