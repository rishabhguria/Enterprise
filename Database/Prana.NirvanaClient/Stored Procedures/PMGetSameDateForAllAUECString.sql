
 
  
/****************************************************************************                                                              
Name :   PMGetSameDateForAllAUECString
Date Created: 23-Apr-2008
Purpose:  Get the same date for all AUEC against the date passed.                                                              
Module Name: PM
Author: Bhupesh Bareja                                                              
Parameters:                                                               
  @date datetime,
  @ErrorMessage varchar(500) output,
  @ErrorNumber int output
Execution StateMent:                                                               
   EXEC [PMGetSameDateForAllAUECString] '04-23-2008'
                                                              
Date Modified:                                                               
Description:                                                                 
Modified By:                                                                 
****************************************************************************/                                                              
CREATE PROCEDURE [dbo].[PMGetSameDateForAllAUECString] (                                                              
 @date datetime,                                                              
 @ErrorMessage varchar(500) output,
 @ErrorNumber int output
)                                                              
AS                                                              
                                                              
SET @ErrorMessage = 'Success'                                                              
SET @ErrorNumber = 0                                                              
                                                              
BEGIN TRY   

DECLARE @auecID int
set @auecID=0

 DECLARE @AllAUECDatesString VARCHAR(MAX)
 Set @AllAUECDatesString = ' '
 DECLARE @AUECSeparator varchar(1)  
 DECLARE @DateSeparator varchar(1)  
  
 SET @AUECSeparator = '~'  
 SET @DateSeparator = '^'  


DECLARE AUECString_Cursor CURSOR FAST_FORWARD FOR                                                    
Select                     
	AUECID
	FROM T_AUEC

   Open AUECString_Cursor

FETCH NEXT FROM AUECString_Cursor INTO
@auecID;
 
WHILE @@fetch_status = 0                                                    
  BEGIN 

--	select
--	@auecID

--print '1'
--print @auecID  
	Set @AllAUECDatesString = @AllAUECDatesString + CONVERT(varchar, @auecID) + @DateSeparator + CONVERT(varchar, @date, 101) + @AUECSeparator

	FETCH NEXT FROM AUECString_Cursor INTO 
	@auecID ;                                                
  END   
           
     
                               
CLOSE AUECString_Cursor
DEALLOCATE AUECString_Cursor

Select @AllAUECDatesString

                                                             
END TRY                                                              
BEGIN CATCH     
 CLOSE AUECString_Cursor
 DEALLOCATE AUECString_Cursor
                                                          
 SET @ErrorMessage = ERROR_MESSAGE();                                                              
 SET @ErrorNumber = Error_number();                                                
 
END CATCH;  





