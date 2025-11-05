
/****************************************************************************
Name :   PMAddNewDataSource
Date Created: 10-oct-2006 
Purpose:  Get all the Company Names and ID's
Author: Sugandh Jain
Execution Statement : exec [PMGetCompanyNameIDList]

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetCompanyNameIDList]
(
			  @ErrorMessage varchar(500) output
			, @ErrorNumber int output 
	)
AS


SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY

Select 
	  PMCompanyID AS [COMPANYID]	
	, TC.ShortName AS [SHORTNAME]			
	, TC.NAME AS [COMPANYNAME]
From 
	PM_Company AS PMC
	INNER JOIN T_Company TC ON PMC.NOMSCompanyID = TC.CompanyID	
WHERE
	PMC.IsActive = 1
Order By 
	[COMPANYNAME] ASC



END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;





