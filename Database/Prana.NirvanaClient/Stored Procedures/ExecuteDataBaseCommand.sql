/****************************************************************************        
Name :   [ExecuteDataBaseCommand]        
Date Created: 26-Mar-2007         
Purpose:  Executes the Database command passed using the exec function
Author: Sugandh        
Parameters:         
 @values Text,        
 @ErrorNumber int output,        
 @ErrorMessage varchar(200) output         
        
Execution Statement :         
     
      
Date Modified: <>
Description:   <>
Modified By:   <>
****************************************************************************/        
CREATE Proc [dbo].[ExecuteDataBaseCommand]        
(        
 @command text,        
 @ErrorNumber int output,        
 @ErrorMessage varchar(200) output        
 )        
AS         
        
SET @ErrorNumber = 0        
SET @ErrorMessage = 'Success'        
        
BEGIN TRY        
        
    
BEGIN TRAN        
exec (@command)           
    
COMMIT TRAN        
        
END TRY        
BEGIN CATCH        
         
 SET @ErrorNumber = ERROR_NUMBER();        
 SET @ErrorMessage = ERROR_MESSAGE();        
         
 ROLLBACK TRAN        
        
END CATCH; 