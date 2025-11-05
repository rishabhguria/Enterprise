CREATE TABLE [dbo].[T_AUECCommissionRules] (
    [RuleID]              INT          IDENTITY (1, 1) NOT NULL,
    [AUECID_FK]           INT          NOT NULL,
    [RuleName]            VARCHAR (50) NOT NULL,
    [ApplyRuletoID_FK]    INT          NOT NULL,
    [RuleDescription]     VARCHAR (50) NULL,
    [CalculationID_FK]    INT          NOT NULL,
    [CurrencyID_FK]       INT          NOT NULL,
    [CommissionRateID_FK] INT          NOT NULL,
    [Commission]          FLOAT (53)   NOT NULL,
    [ApplyCriteria]       INT          NOT NULL,
    [ApplyClrFee]         INT          NOT NULL,
    CONSTRAINT [PK_T_AUECCommissionRules] PRIMARY KEY CLUSTERED ([RuleID] ASC),
    CONSTRAINT [FK_T_AUECCommissionRules_T_ApplyRule] FOREIGN KEY ([ApplyRuletoID_FK]) REFERENCES [dbo].[T_ApplyRule] ([ApplyRuletoId]),
    CONSTRAINT [FK_T_AUECCommissionRules_T_CommissionCalculation] FOREIGN KEY ([CalculationID_FK]) REFERENCES [dbo].[T_CommissionCalculation] ([CommissionCalculationID]),
    CONSTRAINT [FK_T_AUECCommissionRules_T_CommissionRateType] FOREIGN KEY ([CommissionRateID_FK]) REFERENCES [dbo].[T_CommissionRateType] ([CommissionRateID])
);

