




/****************************************************************************
Name :   CheckTriggerExistsInDataBase
Date Created: 26-March-2007 
Purpose:  Check if Trigger exists in the database or not.
Author: Sugandh Jain
Execution Statement : exec PMGetDataSourceTypes

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[CheckTriggerExistsInDataBase]
(
	@TriggerName AS varchar(200),
	@Exists AS bit OUTPUT
	
)
AS
	SET @Exists = 0;	
IF OBJECT_ID(@TriggerName) IS NOT NULL
BEGIN
	SET @Exists = 1;	
END



