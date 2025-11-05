-- This is tamparary table to store the indermediate data during the symbol wise accrual reval entries generation
CREATE TABLE T_IntermediateSymbolLevelAccrualAllActivity (
		[ActivityTypeId_FK] [int] NOT NULL
		,[FKID] [varchar](50) NULL
		,[FundID] [int] NULL
		,[TransactionSource] [int] NOT NULL
		,[ActivitySource] [int] NOT NULL
		,[Symbol] [varchar](100) NULL
		,[Amount] [float] NULL
		,[CurrencyID] [int] NULL
		,[Description] [varchar](3000) NULL
		,[SideMultiplier] [int] NULL
		,[TradeDate] [datetime] NULL
		,[FxRate] [float] NULL
		,[FXConversionMethodOperator] VARCHAR(3) NULL
		,[ActivityState] VARCHAR(50) NULL
		,[ActivityNumber] INT NULL
		,[SubAccountID] INT NULL
		)
