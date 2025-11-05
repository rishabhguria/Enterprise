CREATE TABLE [dbo].[T_OtherFeeRules] (
    [OtherFeeRuleID]        UNIQUEIDENTIFIER NOT NULL,
    [LongFeeRate]           FLOAT (53)       NOT NULL,
    [ShortFeeRate]          FLOAT (53)       NOT NULL,
    [LongCalculationBasis]  INT              NOT NULL,
    [ShortCalculationBasis] INT              NOT NULL,
    [RoundOffPrecision]     SMALLINT         NOT NULL,
    [MaxValue]              FLOAT (53)       NOT NULL,
    [MinValue]              FLOAT (53)       NOT NULL,
    [AUECID]                INT              NOT NULL,
    [FeeTypeID]             INT              NOT NULL,
    [RoundUpPrecision]      SMALLINT         NULL,
    [RoundDownPrecision]    SMALLINT         NULL,
    [FeePrecisionType]      SMALLINT         NULL,
    [IsCriteriaApplied]     BIT              NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_T_OtherFeeRules] PRIMARY KEY CLUSTERED ([OtherFeeRuleID] ASC),
    CONSTRAINT [FK_T_OtherFeeRules_T_AUEC] FOREIGN KEY ([AUECID]) REFERENCES [dbo].[T_AUEC] ([AUECID])
);

