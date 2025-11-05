






/****************************************************************************
Name :   PMGetUnMappedApplicationFundsForCompany
Date Created: 24-Nov-2006 
Purpose:  Gets the ID, shortName and FullName for the companyId passed.
Author: Sugandh Jain
Parameters: 
			@companyID int			
Execution StateMent: 
			EXEC [PMGetUnMappedApplicationFundsForCompany] 1
Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetUnMappedApplicationFundsForCompany]
	(
			@companyID int				  
	)
AS 

BEGIN TRY
SELECT 
	  CompanyFundID
	, FundName
	, FundShortName
FROM 
	T_CompanyFunds
WHERE
	CompanyID = @companyid

END TRY
BEGIN CATCH
--	SET @ERROR = ERROR_NUMBER();
--	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;
--RETURN @ERROR





