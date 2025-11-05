CREATE TABLE [dbo].[T_ClearingFee] (
    [ClearingFeeId]      BIGINT           IDENTITY (1, 1) NOT NULL,
    [CalculationBasedOn] INT              NOT NULL,
    [FeeRate]            FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [MinFee]             FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [RuleId_FK]          UNIQUEIDENTIFIER NOT NULL,
	[IsCriteriaApplied]	 BIT			  DEFAULT ((0)) NOT NULL,
    [BrokerLevelFeeType] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([ClearingFeeId] ASC) WITH (FILLFACTOR = 100),
    FOREIGN KEY ([RuleId_FK]) REFERENCES [dbo].[T_CommissionRules] ([RuleId])
);

