CREATE TABLE [dbo].[PM_CompanyMonthlyAccruals] (
    [CompanyID]    INT          NOT NULL,
    [MonthId]      INT          CONSTRAINT [DF_PM_CompanyMonthlyAccruals_MonthId] DEFAULT ((0)) NOT NULL,
    [Month]        VARCHAR (10) NOT NULL,
    [AccrualValue] FLOAT (53)   NULL,
    [Year]         INT          NOT NULL
);

