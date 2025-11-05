CREATE TABLE [dbo].[T_CompanyMasterFundSubAccountAssociation] (
    [CompanyMaster-SubAccountID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyMasterFundID]        INT NOT NULL,
    [CompanyFundID]              INT NOT NULL,
    CONSTRAINT [PK_T_CompanyMasterFundSubAccountAssociation] PRIMARY KEY CLUSTERED ([CompanyMaster-SubAccountID] ASC),
    CONSTRAINT [FK_T_CompanyMasterFundSubAccountAssociation_T_CompanyFunds] FOREIGN KEY ([CompanyFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_T_CompanyMasterFundSubAccountAssociation_T_CompanyMasterFundSubAccountAssociation] FOREIGN KEY ([CompanyMasterFundID]) REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID])
);

