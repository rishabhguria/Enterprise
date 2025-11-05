CREATE TABLE [dbo].[T_SymbolMapping] (
    [MappedSymbolID]      INT          IDENTITY (1, 1) NOT NULL,
    [AUECID]              INT          NOT NULL,
    [Symbol]              VARCHAR (50) NOT NULL,
    [MappedSymbol]        VARCHAR (50) NOT NULL,
    [CounterPartyVenueID] INT          NOT NULL,
    CONSTRAINT [PK_T_SymbolMapping] PRIMARY KEY CLUSTERED ([MappedSymbolID] ASC)
);

