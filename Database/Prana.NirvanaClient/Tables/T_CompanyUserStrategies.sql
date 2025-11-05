CREATE TABLE [dbo].[T_CompanyUserStrategies] (
    [CompanyUserStrategyID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyStrategyID]     INT NOT NULL,
    [CompanyUserID]         INT NOT NULL,
    CONSTRAINT [PK_T_CompanyUserStrategies] PRIMARY KEY CLUSTERED ([CompanyUserStrategyID] ASC)
);

