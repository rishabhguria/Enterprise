



/****************************************************************************
Name :   CheckTableExistsInDataBase
Date Created: 23-March-2007 
Purpose:  Check if table exists in the database or not.
Author: Sugandh Jain
Execution Statement : exec PMGetDataSourceTypes

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[CheckTableExistsInDataBase]
(
	@TableName AS varchar(200),
	@Exists AS bit OUTPUT ,
	@ColumnCount int OUTPUT
)
AS
	SET @Exists = 0;
	SET @ColumnCount = 0;
IF OBJECT_ID(@tableName) IS NOT NULL
BEGIN

	SET @Exists = 1;
	SELECT  @ColumnCount = COUNT(Column_Name) from 
	information_schema.columns where table_name = @TableName
END


