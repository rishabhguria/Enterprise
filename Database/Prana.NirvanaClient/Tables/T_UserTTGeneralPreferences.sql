CREATE TABLE [dbo].[T_UserTTGeneralPreferences] (
	[UserGeneralPrefID] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY
	,[UserID] INT NOT NULL
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
	,[QuantityType] TINYINT  NULL DEFAULT 0
	,[IsUseRoundLots] BIT NOT NULL DEFAULT 0
	,FOREIGN KEY (UserID) REFERENCES T_CompanyUser(UserID) ON DELETE CASCADE
	)
