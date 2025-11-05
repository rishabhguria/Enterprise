CREATE TABLE [dbo].[T_BenchMark] (
    [BenchMarkSymbol]   VARCHAR (20) NOT NULL,
    [SymbolDisplayName] VARCHAR (50) NULL,
    CONSTRAINT [PK_T_BenchMark] PRIMARY KEY CLUSTERED ([BenchMarkSymbol] ASC)
);

