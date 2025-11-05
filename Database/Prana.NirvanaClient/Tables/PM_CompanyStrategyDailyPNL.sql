CREATE TABLE [dbo].[PM_CompanyStrategyDailyPNL] (
    [CompanyID]                NUMERIC (18) NOT NULL,
    [StrategyID]               NUMERIC (18) NOT NULL,
    [Date]                     DATETIME     NOT NULL,
    [ApplicationRealizedPNL]   FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [PBRealizedPNL]            FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [ApplicationUnrealizedPNL] FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [PBUnrealizedPNL]          FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [DayPNL]                   FLOAT (53)   DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CompanyID] ASC, [StrategyID] ASC, [Date] ASC) WITH (FILLFACTOR = 100)
);

