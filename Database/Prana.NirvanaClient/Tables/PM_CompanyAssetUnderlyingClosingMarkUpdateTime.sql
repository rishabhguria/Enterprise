CREATE TABLE [dbo].[PM_CompanyAssetUnderlyingClosingMarkUpdateTime] (
    [ClosingMarkUpdateTimeID] INT           IDENTITY (1, 1) NOT NULL,
    [CompanyID]               INT           NOT NULL,
    [AssetID]                 INT           NOT NULL,
    [UnderLyingID]            INT           NOT NULL,
    [Time]                    DATETIME      NULL,
    [TimeZone]                VARCHAR (100) NULL
);

