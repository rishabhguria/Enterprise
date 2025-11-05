/****************************************************************************
Name :   PMGetTableTypes
Date Created: 21-dec-2006 
Purpose:  Get all the TableTypes
Author: Bhupesh Bareja
Execution Statement : exec PMGetTableTypes

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetTableTypes]
(
			
			  @ErrorMessage varchar(500) output
			, @ErrorNumber int output 
)
AS

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY

	Select 
		 TableTypeID, TableTypeName
	From 
		PM_TableTypes
	Order By 
		TableTypeID

END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;
