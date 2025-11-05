CREATE TABLE [dbo].[T_CompanyUserMarketDataTypes] (
    [CompanyUserID]    INT NOT NULL,
    [MarketDataTypeID] INT NOT NULL,
    CONSTRAINT [FK_T_CompanyUserMarketDataTypes_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID]),
    CONSTRAINT [FK_T_CompanyUserMarketDataTypes_T_MarketDataType] FOREIGN KEY ([MarketDataTypeID]) REFERENCES [dbo].[T_MarketDataType] ([MarketDataTypeID])
);

