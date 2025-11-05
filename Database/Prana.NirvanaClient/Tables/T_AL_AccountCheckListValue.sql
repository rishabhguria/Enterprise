CREATE TABLE [dbo].[T_AL_AccountCheckListValue]
(
	[Id]					INT IDENTITY (1, 1) NOT NULL,
	[CheckListId]			INT NOT NULL,
	[AccountId]				INT NOT NULL,
	[Value]					DECIMAL (32, 19) NOT NULL,
	CONSTRAINT [PK_T_AL_AccountCheckListValue_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_T_AL_AccountCheckListValue_CheckListId] FOREIGN KEY ([CheckListId]) REFERENCES [dbo].[T_AL_CheckList] ([CheckListId]),
	CONSTRAINT [FK_T_AL_AccountCheckListValue_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
	CONSTRAINT [AK_T_AL_AccountCheckListValue_[CheckListId_AccountId] UNIQUE NONCLUSTERED ([CheckListId] ASC, [AccountId] ASC)
);
