CREATE TABLE [dbo].[T_AL_ProrataFundList] (
    [CheckListId]  INT NOT NULL,
    [PreferenceId] INT NOT NULL,
    [FundId]       INT NOT NULL,
    CONSTRAINT [PK_T_AL_ProrataFundList_PreferenceId_CheckListId_FundId] PRIMARY KEY CLUSTERED ([PreferenceId] ASC, [CheckListId] ASC, [FundId] ASC),
    CONSTRAINT [FK_T_AL_ProrataFundList_FundId] FOREIGN KEY ([FundId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

