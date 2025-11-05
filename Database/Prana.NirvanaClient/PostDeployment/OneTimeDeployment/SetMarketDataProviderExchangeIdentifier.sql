UPDATE [dbo].[T_AUEC] SET [MarketDataProviderExchangeIdentifier] = [ExchangeIdentifier]
WHERE [MarketDataProviderExchangeIdentifier] IS NULL;