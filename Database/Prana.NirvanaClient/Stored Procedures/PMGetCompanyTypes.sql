



/****************************************************************************
Name :   [PMGetCompanyTypes]
Date Created: 14-Dec-2006 
Purpose:  Gets the list of companyTypes. used for PM. 
		  Another SP with same work done is present as [P_GetCompanyTypes],
		  Without the Try .. Catch  thing and error parameters. 
Author: Sugandh Jain
Parameters: 
			@companyID int			
Execution StateMent: 
			EXEC [PMGetCompanyTypes]  '' , 0
Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetCompanyTypes]
(
			
			  @ErrorMessage varchar(500) output
			, @ErrorNumber int output 
	)
AS 


SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
	Select CompanyTypeID, CompanyType
	From T_CompanyType


END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;




