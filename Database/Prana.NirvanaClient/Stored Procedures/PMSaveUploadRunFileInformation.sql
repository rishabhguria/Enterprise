



/****************************************************************************
Name :   [PMSaveUploadRunFileInformation]
Date Created: 27-nov-2006 
Purpose:  Add Update Run Upload Setup items
Author: Sugandh
Parameters: 
	@RunUploadSetupXml	

Execution Statement : 
	exec [PMAddUpdateRunUploadSetup]  1, 1, getdate(), getdate(), 100, 'Successfull', 13, 14, '', 

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE Proc [dbo].[PMSaveUploadRunFileInformation]
(
	  @CompanyUploadSetupID int
	, @Status int
	, @UploadStart DateTime
	, @UploadEnd DateTime
	, @TotalRecords int
	, @StatusDescription nvarchar(4000)
	, @HeaderRowIndex int
	, @FirstRecordIndex int	
	, @UploadID int output
	, @ErrorNumber int output
	, @ErrorMessage varchar(200) output
	)
AS 

SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY

BEGIN TRAN

INSERT  INTO
		PM_UploadRuns 
			(
				
				 CompanyUploadSetUpID
				, Status 
				, UploadStart
				, UploadEnd
				, TotalRecords
				, StatusDescription		
				, HeaderRowIndex			
				, FirstRecordIndex
			)
VALUES
			(
				 
				 @CompanyUploadSetupID 
				, @Status 
				, @UploadStart 
				, @UploadEnd 
				, @TotalRecords 
				, @StatusDescription 
				, @HeaderRowIndex 
				, @FirstRecordIndex 				
			)
SET @UploadID = (select max(uploadid) from PM_UploadRuns)

COMMIT TRAN

END TRY
BEGIN CATCH
	
	SET @ErrorNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	
	ROLLBACK TRAN

END CATCH;



