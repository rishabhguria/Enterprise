CREATE TABLE [dbo].[PM_OTCSymbol] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [Symbol]             VARCHAR (100) NOT NULL,
    [SymbolConventionID] INT           NULL,
    CONSTRAINT [PK_PM_OTCSymbol] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PM_OTCSymbol_PM_SymbolConvention] FOREIGN KEY ([SymbolConventionID]) REFERENCES [dbo].[PM_SymbolConvention] ([ID])
);

