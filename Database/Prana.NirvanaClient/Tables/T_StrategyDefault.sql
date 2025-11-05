CREATE TABLE [dbo].[T_StrategyDefault] (
    [DefaultID]      VARCHAR (200) NOT NULL,
    [DefaultName]    VARCHAR (200) NULL,
    [Strategies]     VARCHAR (200) NULL,
    [StrategyValues] VARCHAR (200) NULL,
    [CompanyUserID]  INT           NULL,
    CONSTRAINT [PK_T_StrategyDefault] PRIMARY KEY CLUSTERED ([DefaultID] ASC),
    CONSTRAINT [FK_T_StrategyDefault_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

