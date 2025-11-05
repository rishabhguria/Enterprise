






/****************************************************************************
Name :   PMAddNewDataSource
Date Created: 24-Nov-2006 
Purpose:  Saves the riskmodelid and allowdialyimport, boolean value.
Author: Sugandh Jain
Parameters: 
			@companyID int			
Execution StateMent: 
			
			EXEC PMSaveCompanyApplicationDetails 1 , 1 , true, '', 0
select * from pm_company 
Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMSaveCompanyApplicationDetails]
	(
			  @CompanyID int	
			, @RiskModelID int
			, @AllowDailyImport bit
			, @ErrorMessage varchar(500) output
			, @ErrorNumber int output 
	)
AS 

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRAN TRAN1
BEGIN TRY
UPDATE 
		 PM_Company 
SET
		  RiskModelID = @RiskModelID
		, AllowDailyImport = @AllowDailyImport
WHERE  
	PMCompanyID = @companyid

COMMIT TRAN TRAN1
END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
ROLLBACK TRAN TRAN1
END CATCH;
--RETURN @ERROR





