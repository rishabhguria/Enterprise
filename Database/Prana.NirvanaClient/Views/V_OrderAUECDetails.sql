CREATE VIEW dbo.V_OrderAUECDetails
AS
SELECT DISTINCT 
                      TOP (100) PERCENT dbo.T_Asset.AssetName, dbo.T_Asset.AssetID, dbo.T_UnderLying.UnderLyingName, dbo.T_UnderLying.UnderLyingID, 
                      dbo.T_AUEC.AUECID, SMData.CurrencyID, dbo.T_Currency.CurrencyName, dbo.T_Exchange.ExchangeID, 
                      dbo.T_Exchange.DisplayName AS ExchangeName, dbo.T_Order.ParentClOrderID AS ClOrderID, dbo.T_Order.TradingAccountID, 
                      dbo.T_AUEC.CountryFlagID
FROM         dbo.T_Order INNER JOIN
					  V_SecMasterData SMData ON SMData.TickerSymbol = T_Order.Symbol INNER JOIN
                      dbo.T_AUEC ON dbo.T_AUEC.AUECID = dbo.T_Order.AUECID INNER JOIN
                      dbo.T_Asset ON dbo.T_Asset.AssetID = dbo.T_AUEC.AssetID INNER JOIN
                      dbo.T_UnderLying ON dbo.T_UnderLying.UnderLyingID = dbo.T_AUEC.UnderLyingID INNER JOIN
                      dbo.T_Exchange ON dbo.T_Exchange.ExchangeID = dbo.T_AUEC.ExchangeID INNER JOIN
                      dbo.T_Currency ON SMData.CurrencyID = dbo.T_Currency.CurrencyID
ORDER BY dbo.T_Asset.AssetID
