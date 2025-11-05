CREATE TABLE [dbo].[T_MarketDataType] (
    [MarketDataTypeID] INT          NOT NULL,
    [MarketDataType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_MarketDataType] PRIMARY KEY CLUSTERED ([MarketDataTypeID] ASC)
);

