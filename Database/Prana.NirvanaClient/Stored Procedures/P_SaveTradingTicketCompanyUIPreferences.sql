CREATE PROCEDURE [dbo].[P_SaveTradingTicketCompanyUIPreferences] (
	@Xml XML
	,@id INT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY
	BEGIN TRANSACTION TRAN1

	DECLARE @handle INT

	EXEC sp_xml_preparedocument @handle OUTPUT
		,@Xml

	CREATE TABLE #XmlItemAssetWise (
		AssetID INT
		,SideID INT
		)

	CREATE TABLE #XmlItemGeneralPrefs (
		CounterPartyID INT
		,VenueID INT
		,OrderTypeID INT
		,TimeInForceID INT
		,ExecutionInstructionID INT
		,HandlingInstructionID INT
		,TradingAccountID INT
		,StrategyID INT
		,AccountID INT
		,IsSettlementCurrencyBase BIT
		,Quantity FLOAT
		,IncrementOnQty FLOAT
		,IncrementOnStop FLOAT
		,IncrementOnLimit FLOAT
		,QuantityType TINYINT
		,IsShowTargetQTY BIT
		,FromControlToControlMapping varchar(max)
		)

	INSERT INTO #XmlItemAssetWise (
		AssetID
		,SideID
		)
	SELECT AssetID
		,SideID
	FROM OPENXML(@handle, '//TradingTicketUIPreferences/DefAssetSides/DefAssetSide', 2) WITH (
			AssetID INT 'Asset'
			,SideID INT 'OrderSide'
			)

	INSERT INTO #XmlItemGeneralPrefs (
		CounterPartyID
		,VenueID
		,OrderTypeID
		,TimeInForceID
		,ExecutionInstructionID
		,HandlingInstructionID
		,TradingAccountID
		,StrategyID
		,AccountID
		,IsSettlementCurrencyBase
		,Quantity
		,IncrementOnQty
		,IncrementOnStop
		,IncrementOnLimit
		,QuantityType
		,IsShowTargetQTY
		,FromControlToControlMapping
		)
	SELECT CounterPartyID
		,VenueID
		,OrderTypeID
		,TimeInForceID
		,ExecutionInstructionID
		,HandlingInstructionID
		,TradingAccountID
		,StrategyID
		,AccountID
		,IsSettlementCurrencyBase
		,Quantity
		,IncrementOnQty
		,IncrementOnStop
		,IncrementOnLimit
		,QuantityType
		,IsShowTargetQTY
		,FromControlToControlMapping
	FROM OPENXML(@handle, '//TradingTicketUIPreferences', 2) WITH (
			CounterPartyID INT 'Broker'
			,VenueID INT 'Venue'
			,OrderTypeID INT 'OrderType'
			,TimeInForceID INT 'TimeInForce'
			,ExecutionInstructionID INT 'ExecutionInstruction'
			,HandlingInstructionID INT 'HandlingInstruction'
			,TradingAccountID INT 'TradingAccount'
			,StrategyID INT 'Strategy'
			,AccountID INT 'Account'
			,IsSettlementCurrencyBase BIT 'IsSettlementCurrencyBase'
			,Quantity FLOAT 'Quantity'
			,IncrementOnQty FLOAT 'IncrementOnQty'
			,IncrementOnStop FLOAT 'IncrementOnStop'
			,IncrementOnLimit FLOAT 'IncrementOnLimit'
			,QuantityType TINYINT 'QuantityType'
			,IsShowTargetQTY BIT 'IsShowTargetQTY'
			,FromControlToControlMapping varchar(max) 'DefTTControlsMapping'
			)

	DELETE
	FROM T_CompanyTTAssetPreferences
	WHERE CompanyID = @id;

	DELETE
	FROM T_CompanyTTGeneralPreferences
	WHERE CompanyID = @id;

	INSERT INTO T_CompanyTTAssetPreferences (
		[CompanyID]
		,[AssetID]
		,[SideID]
		)
	SELECT @id
		,AssetID
		,SideID
	FROM #XmlItemAssetWise

	INSERT INTO T_CompanyTTGeneralPreferences (
		CompanyID
		,CounterPartyID
		,VenueID
		,OrderTypeID
		,TimeInForceID
		,ExecutionInstructionID
		,HandlingInstructionID
		,TradingAccountID
		,StrategyID
		,AccountID
		,IsSettlementCurrencyBase
		,Quantity
		,IncrementOnQty
		,IncrementOnStop
		,IncrementOnLimit
		,QuantityType
		,IsShowTargetQTY
		,FromControlToControlMapping
		)
	SELECT @id
		,CounterPartyID
		,VenueID
		,OrderTypeID
		,TimeInForceID
		,ExecutionInstructionID
		,HandlingInstructionID
		,TradingAccountID
		,StrategyID
		,AccountID
		,IsSettlementCurrencyBase
		,Quantity
		,IncrementOnQty
		,IncrementOnStop
		,IncrementOnLimit
		,QuantityType
		,IsShowTargetQTY
		,FromControlToControlMapping
	FROM #XmlItemGeneralPrefs

	EXEC sp_xml_removedocument @handle

	DROP TABLE #XmlItemAssetWise

	DROP TABLE #XmlItemGeneralPrefs

	COMMIT TRANSACTION TRAN1
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();

	ROLLBACK TRANSACTION TRAN1
END CATCH;