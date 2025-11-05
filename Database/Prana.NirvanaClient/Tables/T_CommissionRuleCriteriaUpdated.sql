CREATE TABLE [dbo].[T_CommissionRuleCriteriaUpdated] (
    [CommissionRuleCriteriaID] INT             IDENTITY (1, 1) NOT NULL,
    [CommissionCriteriaID_FK]  INT             NOT NULL,
    [ValueFrom]                BIGINT          NOT NULL,
    [ValueTo]                  BIGINT          NOT NULL,
    [CommissionRateID_FK]      INT             NOT NULL,
    [CommisionRate]            DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_T_CommissionRuleCriteriaUpdated] PRIMARY KEY CLUSTERED ([CommissionRuleCriteriaID] ASC)
);

