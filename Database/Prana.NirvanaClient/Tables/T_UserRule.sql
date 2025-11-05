CREATE TABLE [dbo].[T_UserRule] (
    [UserRuleID] INT         NOT NULL,
    [UserID]     INT         NOT NULL,
    [RuleID]     INT         NOT NULL,
    [Checked]    VARCHAR (2) NOT NULL,
    CONSTRAINT [PK_T_UserRule] PRIMARY KEY CLUSTERED ([UserRuleID] ASC),
    CONSTRAINT [FK_T_UserRule_T_CompanyUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID]),
    CONSTRAINT [FK_T_UserRule_T_Rules] FOREIGN KEY ([RuleID]) REFERENCES [dbo].[T_Rules] ([RuleID])
);

