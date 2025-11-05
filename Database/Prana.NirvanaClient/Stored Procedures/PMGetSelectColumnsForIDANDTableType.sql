/****************************************************************************          
Name :   [PMGetSelectColumnsForIDANDTableType]          
Date Created: 27-dec-2006           
Purpose:  Get all the Select Columns for specified company and for a specified table type.          
Author: Bhupesh Bareja          
Execution Statement :         
exec [PMGetSelectColumnsForIDANDTableType]         
 @ThirdPartyID = 1 ,         
 @TableTypeID =  2 ,        
 @ErrorMessage =  '',         
 @ErrorNumber = 0        
      
select * from dbo.PM_DataSourceTables      
      
Date Modified:  06-March-2007         
Description:         taking the names of columns in the table from INFORMATION_SCHEMA.TABLES      
     and INFORMATION_SCHEMA.COLUMNS system tables.      
Modified By:         Sugandh      
****************************************************************************/          
CREATE PROCEDURE [dbo].[PMGetSelectColumnsForIDANDTableType]          
(          
  @ThirdPartyID int,          
  @TableTypeID int,          
  @ErrorMessage varchar(500) output ,          
  @ErrorNumber int output          
)          
AS          
BEGIN TRY         
SET @ErrorMessage  = 'Success';        
SET @ErrorNumber = 0;        
        
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
  ThirdPartyID = @ThirdPartyID AND TableTypeID = @TableTypeID   
 AND  
 ColumnName <> 'UploadID'         
      
          
END TRY          
BEGIN CATCH           
 SET @ErrorMessage = ERROR_MESSAGE();          
 SET @ErrorNumber = Error_number();           
END CATCH; 