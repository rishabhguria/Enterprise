/****************************************************************************  
Name :   [PMGetAllApplicationColumns]  
Date Created: 27-nov-2006   
Purpose:  Get all the Application Columns   
Author: Ram Shankar Yadav  
Execution Statement : exec [PMGetAllApplicationColumns] 1  
  
Date Modified: 13th April, 07    
Description: Included one more column Type in fetching.      
Modified By: Bhupesh Bareja      
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetAllApplicationColumns]  
AS  
SELECT   
 ApplicationColumnID,  
 Name as ApplicationColumnName ,
 Type 
   
FROM   
 PM_ApplicationColumns  
ORDER BY 
	Name