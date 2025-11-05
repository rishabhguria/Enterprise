CREATE TABLE [dbo].[PM_DataSourceCurrencies] (
    [DataSourceCurrencyID]   INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]           INT           NOT NULL,
    [DataSourceCurrencyName] NVARCHAR (50) NOT NULL,
    [ApplicationCurrencyID]  INT           NOT NULL,
    CONSTRAINT [PK_PM_DataSourceCurrencies] PRIMARY KEY CLUSTERED ([DataSourceCurrencyID] ASC),
    CONSTRAINT [FK_PM_DataSourceCurrencies_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [CK_PM_DataSourceCurrencies_Unique_DataSourceID_ApplicationCurrencyID] UNIQUE NONCLUSTERED ([ThirdPartyID] ASC, [ApplicationCurrencyID] ASC)
);

