CREATE TABLE [dbo].[T_MarketDataColumns] (
    [MarketDataTypeID] INT          NOT NULL,
    [ModuleID]         INT          NOT NULL,
    [ComponentColumn]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_MarketDataColumns] PRIMARY KEY CLUSTERED ([MarketDataTypeID] ASC, [ModuleID] ASC, [ComponentColumn] ASC),
    CONSTRAINT [FK_T_MarketDataColumns_T_MarketDataType] FOREIGN KEY ([MarketDataTypeID]) REFERENCES [dbo].[T_MarketDataType] ([MarketDataTypeID]),
    CONSTRAINT [FK_T_MarketDataColumns_T_Module] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[T_Module] ([ModuleID])
);

