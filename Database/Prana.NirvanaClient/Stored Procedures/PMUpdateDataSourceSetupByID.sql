/****************************************************************************  
Name :   PMUpdateDataSourceSetupByID  
Date Created: 14-nov-2006   
Purpose:  Update DataSource Setup  
Author: Ram Shankar Yadav  
Parameters:   
 @ID int,  
 @ImportMethod int,  
 @ImportFormat int,  
 @ErrorNumber int output,  
 @ErrorMessage varchar(200) output   
  
Execution Statement :   
 exec PMUpdateDataSourceSetupByID  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
Create Proc [dbo].[PMUpdateDataSourceSetupByID]  
(  
 @ID int,  
 @ImportMethod int,  
 @ImportFormat int,  
 @ErrorNumber int output,  
 @ErrorMessage varchar(200) output  
 )  
AS   
  
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'  
  
BEGIN TRY  
  
BEGIN TRAN  
  
Update T_ThirdParty   
Set ImportMethod = @ImportMethod,  
 ImportFormat = @ImportFormat  
Where  
 ThirdPartyID = @ID  
  
COMMIT TRAN  
  
END TRY  
BEGIN CATCH  
   
 SET @ErrorNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
   
 ROLLBACK TRAN  
  
END CATCH;  
  
  
  
  