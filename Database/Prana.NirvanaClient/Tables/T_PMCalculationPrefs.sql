CREATE TABLE [dbo].[T_PMCalculationPrefs] (
    [PMCalculationPrefsID] INT        IDENTITY (1, 1) NOT NULL,
    [CompanyID]            INT        NOT NULL,
    [FundID]               INT        CONSTRAINT [DF_PMCalculationPrefs_FundID] DEFAULT ((0)) NOT NULL,
    [HighWaterMark]        FLOAT (53) NOT NULL,
    [Stopout]              FLOAT (53) NOT NULL,
    [TraderPayoutPercent]  FLOAT (53) NOT NULL
);

