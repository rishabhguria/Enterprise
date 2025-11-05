CREATE TABLE [dbo].[T_CompanyStrategy] (
    [CompanyStrategyID] INT          IDENTITY (1, 1) NOT NULL,
    [StrategyName]      VARCHAR (50) NOT NULL,
    [StrategyShortName] VARCHAR (50) NOT NULL,
    [CompanyID]         INT          NOT NULL,
    [IsActive]          BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CompanyStrategy] PRIMARY KEY CLUSTERED ([CompanyStrategyID] ASC),
    CONSTRAINT [FK_T_CompanyStrategy_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [C_U_StrategyName] UNIQUE NONCLUSTERED ([StrategyName] ASC, [CompanyID] ASC),
    CONSTRAINT [C_U_StrategyShortName] UNIQUE NONCLUSTERED ([StrategyShortName] ASC, [CompanyID] ASC)
);

