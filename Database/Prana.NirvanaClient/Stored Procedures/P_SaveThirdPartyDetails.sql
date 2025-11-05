CREATE PROCEDURE P_SaveThirdPartyDetails (
	@thirdPartyID INT
	,@timeOfSave DATETIME
	,@generatedFileName VARCHAR(100)
	,@fileID BIGINT
	,@taxlotxml VARCHAR(max)
	)
AS
DECLARE @handle INT
DECLARE @IsL1Data VARCHAR(10)
DECLARE @L1Data INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@taxlotxml

CREATE TABLE #Temp_TaxLOT (TaxLotID VARCHAR(50))

INSERT INTO #Temp_TaxLOT
SELECT TaxLotID
FROM OpenXML(@handle, '/TaxLots/TaxLot', 1) WITH (TaxLotID VARCHAR(50) '@TaxLotID')

SELECT @IsL1Data = IsL1Data
FROM openxml(@handle, '/TaxLots', 2) WITH (IsL1Data VARCHAR(50) '@IsL1Data')

IF (@IsL1Data = 'True')
	SET @L1Data = 1
ELSE
	SET @L1Data = 0

INSERT INTO T_ThirdPartyAllocationFile (
	ThirdPartyId
	,FileName
	,FileId
	,TimeOfSave
	)
VALUES (
	@thirdPartyID
	,@generatedFileName
	,@fileID
	,@timeOfSave
	)

INSERT INTO T_ThirdPartyAllocationFileDetails (
	FileId
	,TaxlotId
	,IsL1Data
	)
SELECT @fileID
	,TaxLotID
	,@L1Data
FROM #Temp_TaxLOT

DROP TABLE #Temp_TaxLOT

EXEC sp_xml_removedocument @handle
