CREATE TABLE [dbo].[PM_FIMATNetPosition] (
    [IsPosition]           VARCHAR (500) NOT NULL,
    [DataSourceFundName]   VARCHAR (500) NOT NULL,
    [InternalAccNumber]    VARCHAR (500) NULL,
    [TradeDate]            DATETIME      NULL,
    [UnderlyingSymbol]     VARCHAR (500) NULL,
    [CUSIP]                VARCHAR (500) NULL,
    [OptionSymbolFull]     VARCHAR (500) NULL,
    [OptionBase]           VARCHAR (500) NULL,
    [OptionSymbolShort]    VARCHAR (500) NULL,
    [OptionExpirationDate] DATETIME      NULL,
    [StrikePrice]          VARCHAR (500) NULL,
    [Type]                 VARCHAR (500) NULL,
    [Quantity]             INT           NOT NULL,
    [MarkPrice]            FLOAT (53)    NOT NULL,
    [Multiplier]           FLOAT (53)    NULL,
    [MarketValue]          FLOAT (53)    NULL,
    [ClearingFirm]         VARCHAR (500) NULL,
    [UnderlyingName]       VARCHAR (500) NULL,
    [AssetTypeName]        VARCHAR (500) NULL,
    [UploadID]             INT           NOT NULL
);

