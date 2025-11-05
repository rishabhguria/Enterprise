CREATE PROCEDURE [dbo].[P_SaveTradingTicketUserUIPreferences] (
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
		,IsUseRoundLots BIT
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
		,IsUseRoundLots
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
		,IsUseRoundLots
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
			,IsUseRoundLots BIT 'IsUseRoundLots'
			)

	DELETE
	FROM T_UserTTAssetPreferences
	WHERE UserID = @id;

	DELETE
	FROM T_UserTTGeneralPreferences
	WHERE UserID = @id;

	INSERT INTO T_UserTTAssetPreferences (
		[UserID]
		,[AssetID]
		,[SideID]
		)
	SELECT @id
		,AssetID
		,SideID
	FROM #XmlItemAssetWise

	INSERT INTO T_UserTTGeneralPreferences (
		UserID
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
		,IsUseRoundLots
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
		,IsUseRoundLots
		
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
