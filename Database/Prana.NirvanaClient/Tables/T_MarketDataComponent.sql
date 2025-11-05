CREATE TABLE [dbo].[T_MarketDataComponent] (
    [MarketDataTypeID] INT NOT NULL,
    [ModuleID]         INT NOT NULL,
    CONSTRAINT [PK_T_MarketDataComponent] PRIMARY KEY CLUSTERED ([MarketDataTypeID] ASC, [ModuleID] ASC),
    CONSTRAINT [FK_T_MarketDataComponent_T_MarketDataType] FOREIGN KEY ([MarketDataTypeID]) REFERENCES [dbo].[T_MarketDataType] ([MarketDataTypeID]),
    CONSTRAINT [FK_T_MarketDataComponent_T_Module] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[T_Module] ([ModuleID])
);

