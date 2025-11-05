




/****************************************************************************
Name :   PMGetAllowDailyImportStatusForUploadClient
Date Created: 23-March-2007 
Purpose:  Check if upload client has the permission for daily  import
Author: Sugandh Jain
Execution Statement : exec PMGetAllowDailyImportStatusForUploadClient 1, 0, 0, ''

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].PMGetAllowDailyImportStatusForUploadClient
(
	@PMCompanyID AS int
	, @Permission AS bit OUTPUT 
	, @ErrorNumber int output
	, @ErrorMessage varchar(100) output
)
AS 

--Declare @Error int

SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'
BEGIN TRY

BEGIN TRAN

SELECT @Permission = AllowDailyImport FROM PM_Company WHERE PMCompanyID = @PMCompanyID

COMMIT TRAN

END TRY
BEGIN CATCH
	
	SET @ErrorNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	
	ROLLBACK TRAN
END CATCH;



