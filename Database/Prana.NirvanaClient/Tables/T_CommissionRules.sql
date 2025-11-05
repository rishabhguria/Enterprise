CREATE TABLE [dbo].[T_CommissionRules] (
    [RuleName]                   VARCHAR (100)    NOT NULL,
    [RuleDescription]            VARCHAR (500)    NULL,
    [ApplyRuleForTrade]          INT              NOT NULL,
    [CalculationBasedOn]         INT              NOT NULL,
    [CommissionRate]             FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [MinCommission]              FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [IsCriteriaApplied]          BIT              NOT NULL,
    [IsClearingFeeApplied]       BIT              NOT NULL,
    [RuleId]                     UNIQUEIDENTIFIER NOT NULL,
    [MaxCommission]              FLOAT (53)       NULL,
    [IsRoundOff]                 BIT              NULL,
    [RoundOffValue]              INT              NULL,
    [CalculationBasedOnForSoft]  INT              NOT NULL,
    [CommissionRateForSoft]      FLOAT (53)       CONSTRAINT [DF_T_CommissionRules_CommissionRateForSoft] DEFAULT ((0.0)) NOT NULL,
    [MinCommissionForSoft]       FLOAT (53)       CONSTRAINT [DF_T_CommissionRules_MinCommissionForSoft] DEFAULT ((0.0)) NOT NULL,
    [MaxCommissionForSoft]       FLOAT (53)       NULL,
    [IsCriteriaAppliedForSoft]   BIT              NOT NULL,
    [IsRoundOffForSoft]          BIT              NULL,
    [RoundOffValueForSoft]       INT              NULL,
    [IsClearingBrokerFeeApplied] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([RuleId] ASC) WITH (FILLFACTOR = 100)
);

