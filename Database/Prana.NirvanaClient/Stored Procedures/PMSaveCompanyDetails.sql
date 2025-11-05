







/****************************************************************************
Name :   PMAddNewDataSource
Date Created: 24-Nov-2006 
Purpose:  Enters the transaction password, number of user licences and adminUser ID in PM_Company
Author: Sugandh Jain
Parameters: 
			@companyID int
		, @NumberOfUserLicences int
		, @AdminLoginName nvarchar(50)
		, @TransactionPassword nvarchar(50)
Execution StateMent: 
			EXEC [PMSaveCompanyDetails] 1, 2, 2, 'sugandh' , '', 0
Select * from pm_company

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMSaveCompanyDetails]
	(
		  @CompanyID int
		, @NumberOfUserLicences int
		, @AdminCompanyUserID int
		, @TransactionPassword nvarchar(50)
		, @ErrorMessage varchar(500) output
		, @ErrorNumber int output 
	)
AS 


SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRAN TRAN1

BEGIN TRY

UPDATE PM_Company  
		SET 
		  NofUserLicenses = @NumberOfUserLicences
		, AdminContactUserID = @AdminCompanyUserID		
		, AdminPassword = @TransactionPassword 
		
WHERE
	PMCompanyID = @CompanyID

COMMIT TRAN TRAN1

END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	ROLLBACK TRAN TRAN1
END CATCH;






