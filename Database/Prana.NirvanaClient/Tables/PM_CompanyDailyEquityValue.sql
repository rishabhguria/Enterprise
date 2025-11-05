CREATE TABLE [dbo].[PM_CompanyDailyEquityValue] (
    [CompanyEquityValueID]    INT        IDENTITY (1, 1) NOT NULL,
    [Date]                    DATETIME   NOT NULL,
    [ApplicationRealizedPL]   FLOAT (53) NULL,
    [ApplicationUnrealizedPL] FLOAT (53) NULL,
    [PrimeBrokerRealizedPL]   FLOAT (53) NULL,
    [PrimeBrokerUnrealizedPL] FLOAT (53) NULL,
    [CompanyID]               INT        NOT NULL,
    [EquityValue]             FLOAT (53) NOT NULL
);

