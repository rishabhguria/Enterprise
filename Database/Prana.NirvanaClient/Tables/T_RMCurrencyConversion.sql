CREATE TABLE [dbo].[T_RMCurrencyConversion] (
    [FromCurrencyID]   INT        NOT NULL,
    [ToCurrencyID]     INT        NOT NULL,
    [ConversionType]   NCHAR (10) NOT NULL,
    [ConversionFactor] FLOAT (53) NOT NULL
);

