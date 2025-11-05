/****************************************************************************      
Name :   [PMGetMapColumnsForID]      
Date Created: 27-nov-2006       
Purpose:  Get all the Columns mapping for specified datasource      
Author: Ram Shankar Yadav      
Execution Statement : exec [PMGetMapColumnsForID] 1      
      
Date Modified:         
Description:           
Modified By:           
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetMapColumnsForID]      
(      
  @ThirdPartyID int,    
  @TableTypeID int    
    
)      
AS      
SELECT       
 a.DataSourceColumnID,      
 a.ColumnName as DataSourceColumnName,      
 ISNULL(a.ApplicationColumnId, -1) AS ApplicationColumnId,      
 ISNULL(b.Name, '--Select--') AS ApplicationColumnName ,    
 a.Locked,  
 a.Type     
         
FROM       
 PM_DataSourceColumns a      
LEFT JOIN       
 PM_ApplicationColumns b      
ON      
 a.ApplicationColumnId = b.ApplicationColumnID      
WHERE      
 a.ThirdPartyID = @ThirdPartyID    
 And    
 a.TableTypeID = @TableTypeID    