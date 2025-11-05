CREATE TABLE [dbo].[T_CVCurrency] (
    [CVCurrencyID]        INT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID] INT NOT NULL,
    [CurrencyID]          INT NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueCurrencyID] PRIMARY KEY CLUSTERED ([CVCurrencyID] ASC)
);

