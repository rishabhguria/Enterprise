
--/***********************************************
--Created by: Suraj nataraj
--Script Date: 09/08/2015
--Desc: For storing data of generated trades based on fileformatID and thirdPartyID on file generation
--http://jira.nirvanasolutions.com:8080/browse/PRANA-10893
--*************************************************/
CREATE PROCEDURE [dbo].[P_UpdateTaxlotState] 
(
	@thirdPartyID INT
	,@fileFormatID INT
	,@taxlotxml VARCHAR(max)
	,@deletedTaxlotxml VARCHAR(max)
	,@GenerateCancelNewForAmend BIT = 0
	) WITH RECOMPILE 
AS

	SET NOCOUNT ON;

--Declare @thirdPartyID INT
--	,@fileFormatID INT
--	,@taxlotxml VARCHAR(max)
--	,@deletedTaxlotxml VARCHAR(max),@GenerateCancelNewForAmend BIT

--	set @thirdPartyID = 69
--	set @fileFormatID =118
--	set @taxlotxml = N'<TaxLots IsL1Data="False"><TaxLot TaxLotID ="21041515271801"/><TaxLot TaxLotID ="21041515271801"/></TaxLots>'
--	set @deletedTaxlotxml = '<TaxLots></TaxLots>'
--	set @GenerateCancelNewForAmend = 1

DECLARE @handle INT
DECLARE @IsL1Data VARCHAR(10)

EXEC sp_xml_preparedocument @handle OUTPUT
	,@taxlotxml

CREATE TABLE #Temp_TaxLOT (TaxLotID VARCHAR(50))

INSERT INTO #Temp_TaxLOT
SELECT TaxLotID
FROM openxml(@handle, '/TaxLots/TaxLot', 1) WITH (TaxLotID VARCHAR(50) '@TaxLotID')

SELECT @IsL1Data = IsL1Data
FROM openxml(@handle, '/TaxLots', 2) WITH (IsL1Data VARCHAR(50) '@IsL1Data')

EXEC sp_xml_preparedocument @handle OUTPUT
	,@deletedTaxlotxml

CREATE TABLE #Temp_DeletedTaxLOT (TaxLotID VARCHAR(50))

INSERT INTO #Temp_DeletedTaxLOT
SELECT TaxLotID
FROM openxml(@handle, '/TaxLots/TaxLot', 1) WITH (TaxLotID VARCHAR(50) '@TaxLotID')


IF (@IsL1Data = 'True')
BEGIN
	UPDATE T_PBWiseTaxlotState
	SET TaxlotState = 1
	WHERE PBID = @thirdPartyID
		AND	FileFormatID = @fileFormatID
		And TaxLotID IN (
			SELECT TaxLotID
			FROM #Temp_TaxLOT
			)

	UPDATE T_DeletedTaxlots
	SET TaxlotState = 1
	WHERE TaxLotID IN (
			SELECT TaxLotID
			FROM #Temp_TaxLOT
			)
		AND PBID = @thirdPartyID
		AND FileFormatID = @fileFormatID

	UPDATE T_Level2Allocation
	SET ExternalTransId = ''
	WHERE TaxLotID IN (
			SELECT TaxLotID
			FROM #Temp_DeletedTaxLOT
			)

	UPDATE PM_Taxlots
	SET ExternalTransId = ''
	WHERE TaxLotID IN (
			SELECT TaxLotID
			FROM #Temp_DeletedTaxLOT
			)
END
ELSE
BEGIN

	Create Table #Temp_TaxlotID_Update
	(
	TaxlotID Varchar(50)
	)

	Insert InTo #Temp_TaxlotID_Update
			SELECT TaxLotID
			FROM T_Level2Allocation
	WHERE Level1AllocationID IN 
	(
					SELECT TaxLotID
					FROM #Temp_TaxLOT
					)

	UPDATE T_PBWiseTaxlotState
	SET TaxlotState = 1
	WHERE TaxLotID IN 
	(
		SELECT TaxLotID
		FROM #Temp_TaxlotID_Update
			)
		AND FileFormatID = @fileFormatID
			

	UPDATE T_DeletedTaxlots
	SET TaxlotState = 1
	WHERE Level1AllocationID IN 
	(
			SELECT TaxLotID
			FROM #Temp_TaxLOT
			)
		AND FileFormatID = @fileFormatID

	Create Table #Temp_TaxlotID_Deleted
	(
	TaxlotID Varchar(50)
	)

	Insert InTo #Temp_TaxlotID_Deleted
			SELECT TaxLotID
			FROM T_Level2Allocation
			WHERE Level1AllocationID IN 
			(
					SELECT TaxLotID
					FROM #Temp_DeletedTaxLOT
					)


	UPDATE T_Level2Allocation
	SET ExternalTransId = ''
	WHERE TaxLotID IN 
	(
		SELECT TaxLotID
		FROM #Temp_TaxlotID_Deleted
			)

	UPDATE PM_Taxlots
	SET ExternalTransId = ''
	WHERE TaxLotID IN 
	(
			SELECT TaxLotID
		FROM #Temp_TaxlotID_Deleted
			)

		Drop Table #Temp_TaxlotID_Update,#Temp_TaxlotID_Deleted
END

IF (@GenerateCancelNewForAmend = 1)
BEGIN
	CREATE TABLE #TempTaxlotID (TaxlotID VARCHAR(50))

	INSERT INTO #TempTaxlotID
	SELECT DISTINCT L2.TaxLotID AS TaxLotID
	FROM T_Level2Allocation L2
	INNER JOIN #Temp_TaxLOT TEMP ON L2.Level1AllocationID = TEMP.TaxLotID

	UPDATE T_ThirdPartyFFGenerationDate
	SET GenerationDate = GETDATE()
		,TradeDate = v.AUECLocalDate
		,SideID = v.OrderSideTagValue
		,CounterPartyID = v.CounterPartyID
		,ExecutedQuantity = V.TaxlotQty
		,AvgPrice = v.AvgPrice
		,SettlmentDate = v.SettlementDate
		,FXRate = v.FXRate
		,Commission = v.Commission
		,OtherBrokerFees = v.OtherBrokerFees
		,StampDuty = v.StampDuty
		,TransactionLevy = v.TransactionLevy
		,ClearingFee = v.ClearingFee
		,MiscFees = v.MiscFees
		,VenueID = v.VenueID
		,FXConversionMethodOperator = v.FXConversionMethodOperator
		,ProcessDate = v.ProcessDate
		,OriginalPurchaseDate = v.OriginalPurchaseDate
		,Description = v.Description
		,AccruedInterest = v.AccruedInterest
		,UnderlyingDelta = 0
		,LotId = v.LotId
		,CommissionAmount = 0
		,CommissionRate = 0
		,SecFee = v.SecFee
		,OccFee = v.OccFee
		,OrfFee = v.OrfFee
		,ClearingBrokerFee = v.ClearingBrokerFee
		,SoftCommission = v.SoftCommission
		,SoftCommissionAmount = 0
		,TransactionType = v.TransactionType
		,SettlCurrency = v.SettlCurrency_Taxlot
		,AmendTaxLotId2 = TGD.AmendTaxLotId1
		,AmendTaxLotId1 = replace(CONVERT(NVARCHAR(20), GETDATE(), 11), '/', '') + replace(CONVERT(NVARCHAR(20), GETDATE(), 114), ':', '') + cast(V.GroupRefID AS NVARCHAR(20)) + cast(FundID AS NVARCHAR(10))
		,TaxOnCommissions = V.TaxOnCommissions
		,GroupID = V.GroupID
		,Level2ID = V.Level2ID
		,ExternalTransId = V.ExternalTransId
		,TransactionSource = V.TransactionSource
	FROM T_ThirdPartyFFGenerationDate TGD
	INNER JOIN V_TaxLots V ON V.Level1AllocationID = TGD.TaxLotID
	INNER JOIN #TempTaxlotID TEMP ON TEMP.TaxlotID = V.TaxLotID
	WHERE TGD.fileFormatID = @FileFormatID
		AND TGD.thirdPartyID = @ThirdPartyID

	INSERT INTO T_ThirdPartyFFGenerationDate
	SELECT V.Level1AllocationID
		,@ThirdPartyID
		,@FileFormatID
		,GETDATE()
		,V.AUECLocalDate
		,V.OrderSideTagValue
		,V.CounterPartyID
		,V.TaxLotQty
		,V.AvgPrice
		,V.SettlementDate
		,V.FXRate_Taxlot
		,V.Commission
		,V.OtherBrokerFees
		,V.StampDuty
		,V.TransactionLevy
		,V.ClearingFee
		,V.MiscFees
		,V.VenueID
		,V.FXConversionMethodOperator_Taxlot
		,V.ProcessDate
		,V.OriginalPurchaseDate
		,V.Description
		,V.AccruedInterest
		,0
		,V.LotId
		,0
		,0
		,V.SecFee
		,V.OccFee
		,V.OrfFee
		,V.ClearingBrokerFee
		,V.SoftCommission
		,0
		,V.TransactionType
		,V.SettlCurrency_Taxlot
		,Replace(CONVERT(NVARCHAR(20), GETDATE(), 11), '/', '') + replace(CONVERT(NVARCHAR(20), GETDATE(), 114), ':', '') + cast(V.GroupRefID AS NVARCHAR(20)) + cast(FundID AS NVARCHAR(10))
		,NULL
		,V.TaxOnCommissions
		,V.GroupID
		,V.Level2ID
		,V.ExternalTransId
		,V.TransactionSource
	FROM V_TaxLots V        
	INNER JOIN #TempTaxlotID TEMP ON V.TaxlotID = TEMP.TaxlotID
	WHERE V.Level1AllocationID NOT IN (
			SELECT TaxLotID
			FROM T_ThirdPartyFFGenerationDate
			WHERE ThirdPartyID = @ThirdPartyID
				AND FileFormatID = @FileFormatID
			)

	---- Delete only those taxlots which are not in T_PBWiseTaxlotState    
	---- This scenario comes when one trade allocated to one fund as previously it was allocated to 2 funds    
	
	Create Table #Temp_Level1ID
		(
	Level1AllocationID Varchar(50)
	)

	Insert InTo #Temp_Level1ID
			Select Level1AllocationID 
			From T_Level2Allocation 
			Where TaxlotID IN
			(
				SELECT TaxlotID
				FROM T_PBWiseTaxlotState
				WHERE FileFormatID = @FileFormatID
				)
				 
	DELETE FROM T_ThirdPartyFFGenerationDate
	FROM T_ThirdPartyFFGenerationDate
	WHERE T_ThirdPartyFFGenerationDate.TaxLotID NOT IN 
		(
			Select Level1AllocationID 
			From #Temp_Level1ID
			)
		AND T_ThirdPartyFFGenerationDate.FileFormatID = @FileFormatID

	DROP TABLE #TempTaxlotID, #Temp_Level1ID
END

EXEC sp_xml_removedocument @handle

DROP TABLE #Temp_TaxLOT ,#Temp_DeletedTaxLOT