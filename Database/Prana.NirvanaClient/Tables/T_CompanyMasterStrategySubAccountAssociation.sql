CREATE TABLE [dbo].[T_CompanyMasterStrategySubAccountAssociation] (
    [CompanyMaster-SubAccountID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyMasterStrategyID]    INT NOT NULL,
    [CompanyStrategyID]          INT NOT NULL,
    CONSTRAINT [PK_T_CompanyMasterStrategySubAccountAssociation] PRIMARY KEY CLUSTERED ([CompanyMaster-SubAccountID] ASC),
    CONSTRAINT [FK_T_CompanyMasterStrategySubAccountAssociation_T_CompanyMasterStrategy] FOREIGN KEY ([CompanyMasterStrategyID]) REFERENCES [dbo].[T_CompanyMasterStrategy] ([CompanyMasterStrategyID]),
    CONSTRAINT [FK_T_CompanyMasterStrategySubAccountAssociation_T_CompanyStrategy] FOREIGN KEY ([CompanyStrategyID]) REFERENCES [dbo].[T_CompanyStrategy] ([CompanyStrategyID])
);

