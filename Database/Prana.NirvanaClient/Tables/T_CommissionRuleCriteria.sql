CREATE TABLE [dbo].[T_CommissionRuleCriteria] (
    [CommissionRuleCriteriaID] INT        IDENTITY (1, 1) NOT NULL,
    [CommissionCriteriaID_FK]  INT        NOT NULL,
    [OperatorID_FK]            INT        NOT NULL,
    [Value]                    INT        NOT NULL,
    [CommissionRateID_FK]      INT        NOT NULL,
    [CommisionRate]            FLOAT (53) NOT NULL,
    [RankID]                   INT        NULL,
    CONSTRAINT [PK_T_CommissionRuleCriteria] PRIMARY KEY CLUSTERED ([CommissionRuleCriteriaID] ASC)
);

