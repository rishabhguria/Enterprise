CREATE TABLE [dbo].[T_RMDefaultAlerts] (
    [RMDefaultID]            INT IDENTITY (1, 1) NOT NULL,
    [AlertTypeID]            INT NOT NULL,
    [RefreshRateCalculation] INT NULL,
    [CompanyID]              INT NOT NULL,
    CONSTRAINT [PK_T_RMDefaultAlerts] PRIMARY KEY CLUSTERED ([RMDefaultID] ASC)
);

