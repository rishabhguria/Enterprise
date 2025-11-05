CREATE TABLE [dbo].[T_OtherFeesCriteria]
(
    [OtherFeesCriteriaId]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [LongValueGreaterThan]       FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [LongValueLessThanOrEqualTo] FLOAT (53)       NOT NULL,
    [LongFeeRate]                FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
	[LongCalculationBasis]        INT              NOT NULL,
    [OtherFeeRuleId_FK]          UNIQUEIDENTIFIER NOT NULL,
	[ShortValueGreaterThan]      FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [ShortValueLessThanOrEqualTo]FLOAT (53)       NOT NULL,
    [ShortFeeRate]               FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
	[ShortCalculationBasis]       INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([OtherFeesCriteriaId] ASC) WITH (FILLFACTOR = 100),
    FOREIGN KEY ([OtherFeeRuleId_FK]) REFERENCES [dbo].[T_OtherFeeRules] ([OtherFeeRuleId])
	--[Id] INT NOT NULL PRIMARY KEY
)
