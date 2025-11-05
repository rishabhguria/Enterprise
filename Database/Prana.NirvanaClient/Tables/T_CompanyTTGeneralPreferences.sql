CREATE TABLE [dbo].[T_CompanyTTGeneralPreferences] (
	[CompanyGeneralPrefID] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY
	,[CompanyID] INT NOT NULL
	,[CounterPartyID] INT NULL
	,[VenueID] INT NULL
	,[OrderTypeID] INT NULL
	,[TimeInForceID] INT NULL
	,[ExecutionInstructionID] INT NULL
	,[HandlingInstructionID] INT NULL
	,[TradingAccountID] INT NULL
	,[StrategyID] INT NULL
	,[AccountID] INT NULL
	,[IsSettlementCurrencyBase] BIT NULL DEFAULT 0
	,[Quantity] FLOAT NULL DEFAULT 0
	,[IncrementOnQty] FLOAT NULL DEFAULT 0
	,[IncrementOnStop] FLOAT NULL DEFAULT 0
	,[IncrementOnLimit] FLOAT NULL DEFAULT 0
	,[QuantityType] TINYINT NULL DEFAULT 0
	,FOREIGN KEY (CompanyID) REFERENCES T_Company(CompanyID)
	,FOREIGN KEY (CounterPartyID) REFERENCES T_CounterParty(CounterPartyID)
	,FOREIGN KEY (VenueID) REFERENCES T_Venue(VenueID)
	,FOREIGN KEY (OrderTypeID) REFERENCES T_OrderType(OrderTypesID)
	,FOREIGN KEY (TimeInForceID) REFERENCES T_TimeInForce(TimeInForceID)
	,FOREIGN KEY (ExecutionInstructionID) REFERENCES T_ExecutionInstructions(ExecutionInstructionsID)
	,FOREIGN KEY (HandlingInstructionID) REFERENCES T_HandlingInstructions(HandlingInstructionsID)
	,FOREIGN KEY (TradingAccountID) REFERENCES T_CompanyTradingAccounts(CompanyTradingAccountsID)
	,[IsShowTargetQTY] BIT NULL DEFAULT 1
	,[FromControlToControlMapping] [varchar](max) DEFAULT (' ') NOT NULL
	)
