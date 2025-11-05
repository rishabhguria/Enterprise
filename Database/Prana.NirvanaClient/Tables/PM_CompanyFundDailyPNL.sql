CREATE TABLE [dbo].[PM_CompanyFundDailyPNL] (
    [CompanyID]                NUMERIC (18) NOT NULL,
    [FundID]                   NUMERIC (18) NOT NULL,
    [Date]                     DATETIME     NOT NULL,
    [ApplicationRealizedPNL]   FLOAT (53)   CONSTRAINT [DF__PM_Compan__Appli__1D27F118] DEFAULT ((0)) NOT NULL,
    [PBRealizedPNL]            FLOAT (53)   CONSTRAINT [DF__PM_Compan__PBRea__20045DC3] DEFAULT ((0)) NOT NULL,
    [ApplicationUnrealizedPNL] FLOAT (53)   CONSTRAINT [DF__PM_Compan__Appli__1E1C1551] DEFAULT ((0)) NOT NULL,
    [PBUnrealizedPNL]          FLOAT (53)   CONSTRAINT [DF__PM_Compan__PBUnr__20F881FC] DEFAULT ((0)) NOT NULL,
    [DayPNL]                   FLOAT (53)   CONSTRAINT [DF__PM_Compan__DayPN__61D22120] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PM_CompanyFundDailyPNL] PRIMARY KEY CLUSTERED ([CompanyID] ASC, [FundID] ASC, [Date] ASC)
);

