CREATE PROCEDURE [dbo].[P_SaveFundWiseExecutingBroker](
	@xmlPreferences NTEXT,
	@companyID INT
)
AS
DECLARE @handle1 INT

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@xmlPreferences

BEGIN TRANSACTION;

BEGIN TRY

--Temporary table that stores Fund wise Executing Broker
	CREATE TABLE #TempMapping (colFundID INT,colBrokerID INT);

	INSERT INTO #TempMapping (colFundID, colBrokerID)
	SELECT FundId, BrokerId		
	FROM openXML(@handle1, 'dsExecutingBrokerMappping/dtExecutingBrokerMappping', 2) 
	WITH (FundId INT, BrokerId INT);

--Update the T_FundWiseExecutingBroker table according to #TempMapping	
	MERGE INTO T_FundWiseExecutingBroker AS Target
	USING (SELECT colFundID, colBrokerID FROM #TempMapping) AS Source
	ON Target.FundID = Source.colFundID AND Target.CompanyId = @companyId
	WHEN MATCHED THEN 
		UPDATE SET Target.BrokerId = Source.colBrokerID
	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (FundID, BrokerId, CompanyId) VALUES (Source.colFundID, Source.colBrokerID, @companyId)
	WHEN NOT MATCHED BY SOURCE AND Target.CompanyId = @companyId THEN 
		DELETE;

	DROP TABLE #TempMapping

	COMMIT TRANSACTION;

	EXEC sp_xml_removedocument @handle1

END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;
END CATCH