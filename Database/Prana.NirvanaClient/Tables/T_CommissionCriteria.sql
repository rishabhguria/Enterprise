CREATE TABLE [dbo].[T_CommissionCriteria] (
    [CommissionCriteriaId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [ValueGreaterThan]       FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [ValueLessThanOrEqualTo] FLOAT (53)       NOT NULL,
    [CommissionRate]         FLOAT (53)       DEFAULT ((0.0)) NOT NULL,
    [RuleId_FK]              UNIQUEIDENTIFIER NOT NULL,
    [CommissionType]         INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([CommissionCriteriaId] ASC) WITH (FILLFACTOR = 100),
    FOREIGN KEY ([RuleId_FK]) REFERENCES [dbo].[T_CommissionRules] ([RuleId])
);

