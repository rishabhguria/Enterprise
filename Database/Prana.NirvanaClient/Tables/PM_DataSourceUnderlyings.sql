CREATE TABLE [dbo].[PM_DataSourceUnderlyings] (
    [DataSourceUnderlyingID]   INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]             INT           NOT NULL,
    [DataSourceUnderlyingName] NVARCHAR (50) NOT NULL,
    [ApplicationUnderlyingID]  INT           NOT NULL,
    CONSTRAINT [PK_PM_DataSourceUnderlyings] PRIMARY KEY CLUSTERED ([DataSourceUnderlyingID] ASC),
    CONSTRAINT [FK_PM_DataSourceUnderlyings_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [CK_PM_DataSourceUnderlyings_Unique_DataSourceID_ApplicationUnderlyingID] UNIQUE NONCLUSTERED ([ThirdPartyID] ASC, [ApplicationUnderlyingID] ASC)
);

