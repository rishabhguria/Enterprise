CREATE PROCEDURE P_SaveDeletedTaxLots_XML (@taxlotxml VARCHAR(max))
AS
DECLARE @handle INT

BEGIN TRAN TRAN1

BEGIN TRY
	EXEC sp_xml_preparedocument @handle OUTPUT
		,@taxlotxml

	CREATE TABLE #Temp_TaxLOT (
		TaxLotID VARCHAR(50)
		,GroupID VARCHAR(50)
		)

	INSERT INTO #Temp_TaxLOT
	SELECT TaxLotID
		,GroupID
	FROM openxml(@handle, '/TaxLots/TaxLot', 1) WITH (
			TaxLotID VARCHAR(50) '@TaxLotID'
			,GroupID VARCHAR(50) '@GroupID'
			)

	--delete from T_DeletedTaxLots where T_DeletedTaxLots.TaxLotID in (Select #Temp_TaxLOT.TaxLotID from #Temp_TaxLOT)                      
	--delete from T_DeletedTaxLots where  GroupID in (Select GroupID from #Temp_TaxLOT)                     
	INSERT INTO T_DeletedTaxLots
	SELECT V_TaxLots.TaxLotID
		,Symbol
		,OrderSideTagValue
		,CounterPartyID
		,VenueID
		,V_TaxLots.AUECID
		,TaxlotQty
		,AvgPrice
		,Level2Percentage
		,AUECLocalDate
		,V_TaxLots.AssetID
		,V_TaxLots.UnderlyingID
		,V_TaxLots.ExchangeID
		,CurrencyID
		,FundID
		,SettlementDate
		,
		--ExpirationDate,                      
		Commission
		,GroupID
		,CumQty
		,Quantity
		,
		--PutOrCall,                      
		OtherBrokerFees
		,3
		,--TaxlotState,                      
		GroupRefID
		,OrderTypeTagValue
		,
		--StrikePrice,                  
		Level1AllocationID
		,StampDuty
		,TransactionLevy
		,ClearingFee
		,TaxOnCommissions
		,MiscFees
		,T_AUEC.Multiplier
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
	FROM V_TaxLots
	LEFT OUTER JOIN T_AUEC ON T_AUEC.AUECID = V_Taxlots.AUECID
	WHERE V_TaxLots.TaxLotID IN (
			SELECT DISTINCT #Temp_TaxLOT.TaxLotID
			FROM #Temp_TaxLOT
			)

	DROP TABLE #Temp_TaxLOT

	COMMIT TRANSACTION TRAN1

	EXEC sp_xml_removedocument @handle
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION TRAN1
END CATCH;
