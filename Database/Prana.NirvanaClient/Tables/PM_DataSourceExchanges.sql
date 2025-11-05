CREATE TABLE [dbo].[PM_DataSourceExchanges] (
    [DataSourceExchangeID]   INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]           INT           NOT NULL,
    [DataSourceExchangeName] NVARCHAR (50) NOT NULL,
    [ApplicationExchangeID]  INT           NOT NULL,
    CONSTRAINT [PK_DataSourceExchanges] PRIMARY KEY CLUSTERED ([DataSourceExchangeID] ASC),
    CONSTRAINT [FK_PM_DataSourceexchanges_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [CK_PM_DataSourceExchanges_Unique_DataSourceID_ApplicationExchangeID] UNIQUE NONCLUSTERED ([ThirdPartyID] ASC, [ApplicationExchangeID] ASC)
);

