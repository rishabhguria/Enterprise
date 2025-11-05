CREATE TABLE [dbo].[T_RMCompanyAlerts] (
    [RMCompanyAlertID]        INT           IDENTITY (1, 1) NOT NULL,
    [CompanyExposureLower]    INT           NOT NULL,
    [CompanyExposureUpper]    INT           NOT NULL,
    [AlertTypeID]             INT           NOT NULL,
    [RefreshRateCalculation]  INT           NULL,
    [RMCompanyOverallLimitID] INT           NOT NULL,
    [AlertMessage]            VARCHAR (100) NOT NULL,
    [EmailAddress]            VARCHAR (200) NOT NULL,
    [BlockTrading]            INT           NULL,
    [CompanyID]               INT           NOT NULL,
    [Rank]                    INT           NOT NULL,
    CONSTRAINT [PK_T_CompanyRMAlerts] PRIMARY KEY CLUSTERED ([RMCompanyAlertID] ASC),
    CONSTRAINT [FK_T_RMCompanyAlerts_T_RMCompanyOverallLimit] FOREIGN KEY ([RMCompanyOverallLimitID]) REFERENCES [dbo].[T_RMCompanyOverallLimit] ([RMCompanyOverallLimitID])
);

