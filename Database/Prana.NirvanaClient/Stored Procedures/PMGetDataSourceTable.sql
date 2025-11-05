/****************************************************************************  
Name :   [PMGetDataSourceTable]  
Date Created: 11-Apr-2007   
Purpose:  Get the row for specified data source id.  
Author: Bhupesh Bareja  
Execution Statement : exec [PMGetDataSourceTable] 1, ' ' 0  
  
Date Modified:   
Description:       
Modified By:       
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetDataSourceTable]  
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
  ThirdPartyID,  
  TableName,  
  TableTypeID  
      
 FROM   
  PM_DataSourceTables  
 WHERE  
  ThirdPartyID = @ThirdPartyID  
  
END TRY  
BEGIN CATCH   
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();   
END CATCH;  