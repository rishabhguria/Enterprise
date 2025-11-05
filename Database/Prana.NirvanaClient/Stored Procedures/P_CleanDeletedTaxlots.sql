
/***********************************************             
Created by: Manvendra Prajapati           
    
Script Date: 06/28/2019            
    
Desc: For deleting data from table T_DeletedTaxlots after execution of P_ReAllocateGroup_XML
Also used to update ExternalTransID back for the taxlots in case of client SS.  
    
https://jira.nirvanasolutions.com:8443/browse/PRANA-32462           
*************************************************/
CREATE PROCEDURE [dbo].[P_CleanDeletedTaxlots] 
(
	@xml NVARCHAR(max)
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
)
AS
BEGIN TRY
DECLARE @handle INT
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

EXEC sp_xml_preparedocument @handle OUTPUT
	,@xml

CREATE TABLE #Temp_Taxlots (TaxLotID VARCHAR(50))

INSERT INTO #Temp_Taxlots
SELECT TaxLotID
FROM openxml(@handle, '/TaxLots/TaxLot', 1) WITH (TaxLotID VARCHAR(50) '@TaxLotID')

EXEC sp_xml_preparedocument @handle OUTPUT

SELECT TDT.TaxLotID
	,TDT.ExternalTransId
INTO #Temp_DeletedTaxlots
FROM T_DeletedTaxlots TDT
INNER JOIN #Temp_Taxlots TT ON 
(TT.TaxLotID = TDT.TaxLotID )

/*                 
 Modified By: Sandeep as on August 03, 2014                
 We need ExternalTransID from T_DeletedTaxlots if user re-allocate trades multiple times,                 
 we face this use case while testing SS AXYS EOD file                
*/
IF EXISTS (SELECT 1 FROM #Temp_DeletedTaxlots)
BEGIN

	UPDATE T_Level2Allocation
	SET ExternalTransID = D.ExternalTransID
	FROM T_Level2Allocation
	INNER JOIN #Temp_DeletedTaxlots D ON T_Level2Allocation.TaxlotID = D.TaxlotID
	Where  D.ExternalTransId <> ''

	--UPDATE PM_Taxlots
	--SET ExternalTransID = D.ExternalTransID		
	--FROM T_Level2Allocation
	--INNER JOIN #Temp_DeletedTaxlots D ON T_Level2Allocation.TaxlotID = D.TaxlotID
	--where  D.ExternalTransId<>''

	UPDATE PT
	SET ExternalTransID = D.ExternalTransID		
	FROM PM_Taxlots PT
	INNER JOIN #Temp_DeletedTaxlots D ON PT.TaxlotID = D.TaxlotID
	Where  D.ExternalTransId <> ''

END

DELETE TDT
FROM T_DeletedTaxlots TDT
INNER JOIN #Temp_DeletedTaxlots TT ON TDT.TaxLotID = TT.TaxLotID
--where TDT.TaxLotState=1 or TT.ExternalTransId=''

EXEC sp_xml_removedocument @handle
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
		-- ROLLBACK TRANSACTION TRAN1                                                                                                                                                                                             
END CATCH;