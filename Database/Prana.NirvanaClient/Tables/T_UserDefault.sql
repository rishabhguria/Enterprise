CREATE TABLE [dbo].[T_UserDefault] (
    [UserDefaultID] INT NOT NULL,
    [UserID]        INT NOT NULL,
    [DefaultID]     INT NULL,
    CONSTRAINT [PK_T_UserDefault] PRIMARY KEY CLUSTERED ([UserDefaultID] ASC),
    CONSTRAINT [FK_T_UserDefault_T_CompanyUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID]),
    CONSTRAINT [FK_T_UserDefault_T_Defaults] FOREIGN KEY ([DefaultID]) REFERENCES [dbo].[T_FundStrategyDefaults] ([DefaultID])
);

