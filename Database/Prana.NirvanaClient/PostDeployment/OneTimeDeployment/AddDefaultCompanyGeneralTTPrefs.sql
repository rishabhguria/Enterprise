IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyTTGeneralPreferences'
			AND COLUMN_NAME = 'CompanyGeneralPrefID'
		)
BEGIN
	DECLARE @companyID INT
	DECLARE @counterPartyID INT
	DECLARE @venueID INT
	DECLARE @orderTypeID INT
	DECLARE @TimeInForceID INT
	DECLARE @ExecutionInstructionID INT
	DECLARE @HandlingInstructionID INT
	DECLARE @TradingAccountID INT
	DECLARE @AccountID INT
	DECLARE @StrategyID INT

	SET @companyID = (
			SELECT TOP 1 CompanyID
			FROM T_Company
			WHERE CompanyID > 0
			)
	SET @counterPartyID = (
			SELECT TOP 1 CounterPartyID
			FROM T_CompanyCounterPartyVenues where CounterPartyID > 0
			)
	SET @venueID = (
			SELECT TOP 1 VenueID
			FROM T_CompanyCounterPartyVenues
			INNER JOIN T_CounterPartyVenue
				ON T_CounterPartyVenue.CounterPartyID = T_CompanyCounterPartyVenues.CounterPartyID
			WHERE T_CounterPartyVenue.CounterPartyID = @counterPartyID
				AND CompanyID = @companyID
			)
	SET @orderTypeID = (
			SELECT TOP 1 OrderTypeID
			FROM T_CompanyOrderTypes
			)
	SET @TimeInForceID = (
			SELECT TOP 1 TimeInForceID
			FROM T_TimeInForce
			WHERE TimeInForce = 'Day'
			)
	SET @ExecutionInstructionID = (
			SELECT TOP 1 ExecutionInstructionsID
			FROM T_ExecutionInstructions
			WHERE ExecutionInstructions = 'Do not reduce'
			)
	SET @HandlingInstructionID = (
			SELECT TOP 1 HandlingInstructionsID
			FROM T_HandlingInstructions
			WHERE HandlingInstructions = 'Manual'
			)
	SET @AccountID = -2147483648
	SET @TradingAccountID = (
			SELECT TOP 1 CompanyTradingAccountsID
			FROM T_CompanyTradingAccounts
			)
	 SET @StrategyID = (
			SELECT TOP 1 CompanyStrategyID
			FROM T_CompanyStrategy where IsActive = 1
			)

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
		,Quantity
		,IncrementOnQty
		,IncrementOnStop
		,IncrementOnLimit
		,IsShowTargetQTY
		,FromControlToControlMapping
		)
	VALUES (
		@companyID
		,@counterPartyID
		,@venueID
		,@orderTypeID
		,@TimeInForceID
		,@ExecutionInstructionID
		,@HandlingInstructionID
		,@TradingAccountID
		,@StrategyID
		,@AccountID
		,0
		,0
		,0
		,0
		,1
		,' ' 
		)
END
