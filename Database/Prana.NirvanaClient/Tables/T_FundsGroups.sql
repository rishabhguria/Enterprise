CREATE TABLE [dbo].[T_FundsGroups] (
    [FundGroupID] INT          IDENTITY (1, 1) NOT NULL,
    [FundID]      INT          NULL,
    [GroupID]     VARCHAR (30) NULL,
    CONSTRAINT [PK_T_FundsGroups] PRIMARY KEY CLUSTERED ([FundGroupID] ASC),
    CONSTRAINT [FK_T_FundsGroups_T_CompanyFunds] FOREIGN KEY ([FundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

