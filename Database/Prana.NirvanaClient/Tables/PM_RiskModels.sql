CREATE TABLE [dbo].[PM_RiskModels] (
    [RiskModelID]   INT            NOT NULL,
    [RiskModelName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PM_RiskModels] PRIMARY KEY CLUSTERED ([RiskModelID] ASC)
);

