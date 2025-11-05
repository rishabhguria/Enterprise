/****************************************************************************  
Name :   PMGetDataSourceSetupByID  
Date Created: 27-nov-2006   
Purpose:  Get DataSource Setup details  
Author: Ram Shankar Yadav  
Parameters:   
 @ID  
Execution Statement :   
 exec PMGetDataSourceSetupByID 1  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
Create Proc [dbo].[PMGetDataSourceSetupByID]  
(  
 @ID int  
)  
AS  
  
SELECT   
 ImportMethod,   
 ImportFormat  
FROM  
 T_ThirdParty  
WHERE  
 ThirdPartyID = @ID  