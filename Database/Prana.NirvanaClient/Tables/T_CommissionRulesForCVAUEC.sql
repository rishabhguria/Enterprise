CREATE TABLE [dbo].[T_CommissionRulesForCVAUEC] (
    [CVAUECRuleId]    INT              IDENTITY (1, 1) NOT NULL,
    [CVId_FK]         INT              NULL,
    [AUECId_FK]       INT              NULL,
    [FundId_FK]       INT              NULL,
    [SingleRuleId_FK] UNIQUEIDENTIFIER NULL,
    [BasketRuleId_FK] UNIQUEIDENTIFIER NULL,
    [CompanyID]       INT              NULL,
    PRIMARY KEY CLUSTERED ([CVAUECRuleId] ASC) WITH (FILLFACTOR = 100)
);

