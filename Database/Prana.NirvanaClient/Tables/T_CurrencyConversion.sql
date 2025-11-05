CREATE TABLE [dbo].[T_CurrencyConversion] (
    [FromCurrencyID]   INT           NOT NULL,
    [ToCurrencyID]     INT           NOT NULL,
    [ConversionType]   BIT           CONSTRAINT [DF_T_CurrencyConversion_ConversionType] DEFAULT ((1)) NOT NULL,
    [ConversionFactor] FLOAT (53)    NOT NULL,
    [Symbol]           VARCHAR (100) NULL,
    [Date]             DATETIME      NOT NULL,
    CONSTRAINT [FK_T_CurrencyConversion_T_Currency] FOREIGN KEY ([FromCurrencyID]) REFERENCES [dbo].[T_Currency] ([CurrencyID]),
    CONSTRAINT [FK_T_CurrencyConversion_T_Currency1] FOREIGN KEY ([ToCurrencyID]) REFERENCES [dbo].[T_Currency] ([CurrencyID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for Direct and 0 for InDirect conversion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_CurrencyConversion', @level2type = N'COLUMN', @level2name = N'ConversionType';

