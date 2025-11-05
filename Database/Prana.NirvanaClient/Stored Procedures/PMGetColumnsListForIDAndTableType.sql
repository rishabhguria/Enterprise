/****************************************************************************          
Name :   [PMGetColumnsListForIDAndTableType]          
Date Created: 07-Mar-2007           
Purpose:  Get all the Columns List for specified datasource     
   and for a specified table type.          
Author: Sugandh         
Execution Statement :         
exec [PMGetColumnsListForIDAndTableType]         
 @ThirdPartyID = 1 ,         
 @TableTypeID =  2 ,        
 @ErrorMessage =  '',         
 @ErrorNumber = 0        
      
select * from dbo.PM_DataSourceTables      
Select          
  c.*  
From       
 INFORMATION_SCHEMA.TABLES t       
 Join INFORMATION_SCHEMA.COLUMNS c On t.TABLE_CATALOG = c.TABLE_CATALOG And        
 t.TABLE_SCHEMA = c.TABLE_SCHEMA And       
 t.TABLE_NAME = c.TABLE_NAME And       
 t.TABLE_TYPE = 'BASE TABLE'            
WHERE       
 C.Table_Name = 'pm_csfbnetposition'  
  
Date Modified:  06-March-2007         
Description:         taking the names of columns in the table from INFORMATION_SCHEMA.TABLES      
     and INFORMATION_SCHEMA.COLUMNS system tables.      
Modified By:         Sugandh      
****************************************************************************/          
CREATE PROCEDURE [dbo].[PMGetColumnsListForIDAndTableType]          
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
      
DECLARE @TableName varchar(100);      
SET @TableName = (      
SELECT TOP(1)    
 TableName       
FROM       
 PM_DataSourceTables       
WHERE       
 ThirdPartyID = @ThirdPartyID      
 AND      
 TableTypeID = @TableTypeID      
)      
      
      
Select          
  c.COLUMN_NAME , C.Data_Type       
From       
 INFORMATION_SCHEMA.TABLES t       
 Join INFORMATION_SCHEMA.COLUMNS c On t.TABLE_CATALOG = c.TABLE_CATALOG And        
 t.TABLE_SCHEMA = c.TABLE_SCHEMA And       
 t.TABLE_NAME = c.TABLE_NAME And       
 t.TABLE_TYPE = 'BASE TABLE'       
       
WHERE       
 C.Table_Name = @TableName      
     
          
END TRY          
BEGIN CATCH           
 SET @ErrorMessage = ERROR_MESSAGE();          
 SET @ErrorNumber = Error_number();           
END CATCH; 