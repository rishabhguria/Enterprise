
/****************************************************************************
Name :   PMAddUpdateSetupTradeRecon
Date Created: 30-nov-2006 
Purpose:  Add Update AssetMappings Columns
Author: Ram Shankar Yadav
Parameters: 
	@Xml nText,
	@ErrorNumber int output,
	@ErrorMessage varchar(200) output
Execution Statement : 
	exec PMAddUpdateSetupTradeRecon '<xml><element>value</element></xml>' ,''

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE Proc [dbo].[PMAddUpdateSetupTradeRecon]
(
	@Xml nText,
	@ErrorNumber int output,
	@ErrorMessage varchar(200) output
	)
AS 

SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY

BEGIN TRAN

DECLARE @handle int   
exec sp_xml_preparedocument @handle OUTPUT,@Xml   

--This code updates old data.
UPDATE PM_CompanyReconColumns 
SET 
	PM_CompanyReconColumns.DataSourceColumnID = XmlItem.DataSourceColumnID,
	PM_CompanyReconColumns.IncludeAsCash = XmlItem.IsIncludedAsCash,
	PM_CompanyReconColumns.Type = XmlItem.Type,
	PM_CompanyReconColumns.AcceptableDeviation = XmlItem.AcceptableDeviation,
	PM_CompanyReconColumns.DeviationSign = XmlItem.DeviationSign,
	PM_CompanyReconColumns.AcceptDataFrom = XmlItem.AcceptDataFrom
FROM 
	OPENXML(@handle, '//AppReconciliedColumn', 2)   
	WITH 
		(ID Integer, DataSourceColumnID Integer, IsIncludedAsCash bit, Type bit, DeviationSign Integer, AcceptableDeviation Integer, AcceptDataFrom bit)  XmlItem
	WHERE 
		PM_CompanyReconColumns.CompanyReconColumnID = XmlItem.ID

--This code inserts new data.

Insert Into PM_CompanyReconColumns(DataSourceColumnID, IncludeAsCash, Type, AcceptableDeviation, DeviationSign, AcceptDataFrom)
SELECT DataSourceColumnID, IsIncludedAsCash, Type, AcceptableDeviation, DeviationSign, AcceptDataFrom
FROM 
	OPENXML(@handle, '//AppReconciliedColumn', 2)   
		WITH 
		(ID Integer, DataSourceColumnID Integer, IsIncludedAsCash bit, Type bit, DeviationSign Integer, AcceptableDeviation Integer, AcceptDataFrom bit)  XmlItem
	WHERE 
		XmlItem.ID Not IN (Select CompanyReconColumnID from PM_CompanyReconColumns)

EXEC sp_xml_removedocument @handle

COMMIT TRAN

END TRY
BEGIN CATCH
	
	SET @ErrorNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	
	ROLLBACK TRAN

END CATCH;

