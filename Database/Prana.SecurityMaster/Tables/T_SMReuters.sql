CREATE TABLE [dbo].[T_SMReuters] (
    [AUECID]            INT              NULL,
    [ExchangeID]        INT              NULL,
    [ReutersSymbol]     VARCHAR (100)    NULL,
    [ReutersPK]         INT              IDENTITY (1, 1) NOT NULL,
    [ISPrimaryExchange] BIT              NOT NULL,
    [Symbol_PK]         BIGINT           NULL,
    [CorpActionID]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_T_SMReuters] PRIMARY KEY CLUSTERED ([ReutersPK] ASC),
    CONSTRAINT [FK_T_SMReuters_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMReuters_16_222623836__K7_K6_3]
    ON [dbo].[T_SMReuters]([Symbol_PK] ASC, [ISPrimaryExchange] ASC)
    INCLUDE([ReutersSymbol]);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMReuters_16_222623836__K7_K6_4]
    ON [dbo].[T_SMReuters]([Symbol_PK] ASC, [ISPrimaryExchange] ASC)
    INCLUDE([ReutersSymbol]);

