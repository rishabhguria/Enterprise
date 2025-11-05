CREATE TABLE [dbo].[PM_CompanyBaseEquityValue] (
    [CompanyBaseEquityValueID] INT        IDENTITY (1, 1) NOT NULL,
    [CompanyID]                INT        NOT NULL,
    [BaseEquityValue]          FLOAT (53) NOT NULL,
    [BaseEquityDate]           DATETIME   NOT NULL
);

