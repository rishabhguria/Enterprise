CREATE TABLE [dbo].[PM_DataSourceFunds] (
    [DataSourceFundID]   INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]       INT           NOT NULL,
    [DataSourceFundName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DataSourceFunds] PRIMARY KEY CLUSTERED ([DataSourceFundID] ASC),
    CONSTRAINT [FK_PM_DataSourceFunds_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

