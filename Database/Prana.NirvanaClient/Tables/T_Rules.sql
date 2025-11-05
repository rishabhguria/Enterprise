CREATE TABLE [dbo].[T_Rules] (
    [RuleID]     INT          NOT NULL,
    [Rule]       VARCHAR (50) NULL,
    [RuleTypeID] INT          NULL,
    CONSTRAINT [PK_T_Rules] PRIMARY KEY CLUSTERED ([RuleID] ASC),
    CONSTRAINT [FK_T_Rules_T_RuleType] FOREIGN KEY ([RuleTypeID]) REFERENCES [dbo].[T_RuleType] ([RuleTypeID])
);

