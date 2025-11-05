CREATE TABLE [dbo].[T_CommissionCriteria_old] (
    [CommissionCriteriaID]       INT        IDENTITY (1, 1) NOT NULL,
    [RuleID_FK]                  INT        NOT NULL,
    [CommissionCalculationID_FK] INT        NOT NULL,
    [MinimumCommissionRate]      FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommissionCriteriaID] ASC) WITH (FILLFACTOR = 100),
    FOREIGN KEY ([CommissionCalculationID_FK]) REFERENCES [dbo].[T_CommissionCalculation] ([CommissionCalculationID]),
    FOREIGN KEY ([RuleID_FK]) REFERENCES [dbo].[T_AUECCommissionRules] ([RuleID])
);

