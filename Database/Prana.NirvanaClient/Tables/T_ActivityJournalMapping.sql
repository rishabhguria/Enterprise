CREATE TABLE [dbo].[T_ActivityJournalMapping] (
    [ActivityTypeId_FK] INT     NOT NULL,
    [AmountTypeId_FK]   INT     NOT NULL,
    [DebitAccount]      INT     NULL,
    [CreditAccount]     INT     NULL,
    [CashValueType]     TINYINT DEFAULT ((0)) NOT NULL,
    [ActivityDateType]  INT     DEFAULT ((1)) NOT NULL,
    [Id]                INT     IDENTITY (1, 1) NOT NULL,
    [Description] 		VARCHAR(500) NULL,
	[IsEnabled] BIT DEFAULT 1 NOT NULL, 
    CONSTRAINT [PK_T_ActivityJournalMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_ActivityJournalMapping_T_ActivityAmountType] FOREIGN KEY ([AmountTypeId_FK]) REFERENCES [dbo].[T_ActivityAmountType] ([AmountTypeId]),
    CONSTRAINT [FK_T_ActivityJournalMapping_T_ActivityType] FOREIGN KEY ([ActivityTypeId_FK]) REFERENCES [dbo].[T_ActivityType] ([ActivityTypeId]),
    CONSTRAINT [FK_T_ActivityJournalMapping_T_SubAccounts] FOREIGN KEY ([DebitAccount]) REFERENCES [dbo].[T_SubAccounts] ([SubAccountID]),
    CONSTRAINT [FK_T_ActivityJournalMapping_T_SubAccounts1] FOREIGN KEY ([CreditAccount]) REFERENCES [dbo].[T_SubAccounts] ([SubAccountID])
);

