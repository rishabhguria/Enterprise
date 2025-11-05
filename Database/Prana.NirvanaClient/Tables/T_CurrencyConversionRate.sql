CREATE TABLE [dbo].[T_CurrencyConversionRate] (
    [ConversionRateID]  INT        IDENTITY (1, 1) NOT NULL,
    [CurrencyPairID_FK] INT        NOT NULL,
    [ConversionRate]    FLOAT (53) NOT NULL,
    [Date]              DATETIME   NOT NULL,
    [FundID]            INT        DEFAULT ((0)) NOT NULL,
    [SourceID]          INT        DEFAULT ((0)) NOT NULL,
    [IsApproved]        BIT        DEFAULT ((0)) NOT NULL,
    [AmendedRate]       FLOAT (53) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ConversionRateID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK__T_Currenc__Curre__3865F739] FOREIGN KEY ([CurrencyPairID_FK]) REFERENCES [dbo].[T_CurrencyStandardPairs] ([CurrencyPairID])
);


GO
CREATE NONCLUSTERED INDEX [IX_T_CurrencyConversionRate]
    ON [dbo].[T_CurrencyConversionRate]([Date] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_T_CurrencyConversionRate_1]
    ON [dbo].[T_CurrencyConversionRate]([CurrencyPairID_FK] ASC);

