/****************************************************************************    
Name :   PMAddNewDataSource    
Date Created: 06-oct-2006     
Purpose:  Adds new data source full name and short name to table t_thirdparty    
Author: Sugandh Jain    
Parameters:     
   @FullName Varchar(50)     
   @ShortName varchar(50)    
Execution StateMent:     
       
   EXEC PMAddNewDataSource 'Sugandh Jain', 'SJ'    
    
Date Modified: <DateModified>     
Description:     <DescriptionOfChange>     
Modified By:     <ModifiedBy>     
****************************************************************************/    
Create PROCEDURE [dbo].[PMAddNewDataSource]    
 (    
    @FullName varchar(50)    
  , @ShortName varchar(50)    
  , @ErrorNumber int output    
  , @ErrorMessage varchar(100) output    
 )    
AS     
    
--Declare @Error int    
    
SET @ErrorNumber = 0    
SET @ErrorMessage = 'Success'    
BEGIN TRY    
    
BEGIN TRAN    
    
INSERT  INTO    
  t_thirdparty     
   (    
      thirdpartyName    
    , ShortName    
   )    
VALUES    
   (    
    @FullName    
    , @ShortName    
   )    
    
COMMIT TRAN    
    
END TRY    
BEGIN CATCH    
     
 SET @ErrorNumber = ERROR_NUMBER();    
 SET @ErrorMessage = ERROR_MESSAGE();    
     
 ROLLBACK TRAN    
END CATCH;    
    
    
    
    