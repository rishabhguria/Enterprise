/****************************************************************************  
Name :   [PMGetDataSourceTableValue]  
Date Created: 28-dec-2006   
Purpose:  Get the row for specified table type adn data source id.  
Author: Bhupesh Bareja  
Execution Statement : exec [PMGetDataSourceTableValue] 1 2  
  
Date Modified:   
Description:       
Modified By:       
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetDataSourceTableValue]  
(  
  @ThirdPartyID int,  
  @TableTypeID int,  
  @ErrorMessage varchar(500) output,  
  @ErrorNumber int output  
)  
AS  
SET @ErrorMessage = 'Success'  
SET @ErrorNumber = 0  
BEGIN TRY  
 SELECT   
  ThirdPartyID,  
  TableName,  
  TableTypeID  
      
 FROM   
  PM_DataSourceTables  
 WHERE  
  ThirdPartyID = @ThirdPartyID AND TableTypeID = @TableTypeID  
  
END TRY  
BEGIN CATCH   
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();   
END CATCH;  