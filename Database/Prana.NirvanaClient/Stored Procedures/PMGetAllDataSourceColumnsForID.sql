/****************************************************************************  
Name :   [PMGetAllDataSourceColumnsForID]  
Date Created: 27-nov-2006   
Purpose:  Get all the Columns for specified datasource  
Author: Ram Shankar Yadav  
Execution Statement : exec [PMGetAllDataSourceColumnsForID] 1  
  
Date Modified:     
Description:       
Modified By:       
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetAllDataSourceColumnsForID]  
(  
  @ThirdPartyID int  
)  
AS  
SELECT   
 DataSourceColumnID,  
 ColumnName as DataSourceColumnName  
   
FROM   
 PM_DataSourceColumns  
WHERE  
 ThirdPartyID = @ThirdPartyID  
  
  