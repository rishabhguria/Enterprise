CREATE TABLE [dbo].[T_CompanyThirdPartyCVCommissionRules] (
    [CompanyFundID]           INT    NOT NULL,
    [CompanyCounterPartyCVID] INT    NOT NULL,
    [CVAUECID]                BIGINT NOT NULL,
    [SingleRuleID]            INT    NULL,
    [BasketRuleID]            INT    NULL,
    CONSTRAINT [FK_T_CompanyThirdPartyCVCommissionRules_T_CompanyCounterPartyVenues] FOREIGN KEY ([CompanyCounterPartyCVID]) REFERENCES [dbo].[T_CompanyCounterPartyVenues] ([CompanyCounterPartyCVID]),
    CONSTRAINT [FK_T_CompanyThirdPartyCVCommissionRules_T_CompanyFunds] FOREIGN KEY ([CompanyFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_T_CompanyThirdPartyCVCommissionRules_T_CVAUEC] FOREIGN KEY ([CVAUECID]) REFERENCES [dbo].[T_CVAUEC] ([CVAUECID])
);

