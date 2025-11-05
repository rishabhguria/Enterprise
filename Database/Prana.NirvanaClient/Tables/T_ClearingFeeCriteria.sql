CREATE TABLE [dbo].[T_ClearingFeeCriteria]
(
	[ClearingFeeCriteriaID]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ValueGreaterThan]       FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [ValueLessThanOrEqualTo] FLOAT (53)       NOT NULL,
    [ClearingFeeRate]         FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [ClearingFeeType]         INT              NOT NULL,
	[RuleId_FK]              UNIQUEIDENTIFIER NOT NULL
    PRIMARY KEY CLUSTERED ([ClearingFeeCriteriaID] ASC) WITH (FILLFACTOR = 100),
	FOREIGN KEY ([RuleId_FK]) REFERENCES [dbo].[T_CommissionRules] ([RuleId])
)
