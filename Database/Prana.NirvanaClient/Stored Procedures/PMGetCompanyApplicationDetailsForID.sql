


/****************************************************************************
Name :   PMAddNewDataSource
Date Created: 24-Nov-2006 
Purpose:  Gets the riskmodelid, and isDailyimport allowed permission for the user.
Author: Sugandh Jain
Parameters: 
			@companyID int			
Execution StateMent: 
			
			EXEC GetCompanyApplicationDetailsForID 1
select * from pm_company 
Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetCompanyApplicationDetailsForID]
	(
		  @companyID int	
		, @ErrorMessage varchar(500) output
		, @ErrorNumber int output 	
	)
AS 

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0


BEGIN TRY
SELECT 
		  RM.RiskModelID 
		, RM.RiskModelName
		, C.AllowDailyImport
FROM 
	PM_Company C 
	LEFT OUTER JOIN PM_RiskModels RM ON C.RiskModelID = RM.RiskModelID
WHERE
	C.PMCompanyID = @companyid
	AND
	C.IsActive = 1

END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;







