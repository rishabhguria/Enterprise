CREATE TABLE [dbo].[T_CurrencyStandardPairs] (
    [CurrencyPairID]  INT           IDENTITY (1, 1) NOT NULL,
    [FromCurrencyID]  INT           NOT NULL,
    [ToCurrencyID]    INT           NOT NULL,
    [eSignalSymbol]   VARCHAR (100) NOT NULL,
    [BloombergSymbol] VARCHAR (100) DEFAULT ('') NULL,
    CONSTRAINT [PK__T_CurrencyStanda__3771D300] PRIMARY KEY CLUSTERED ([CurrencyPairID] ASC) WITH (FILLFACTOR = 100)
);

