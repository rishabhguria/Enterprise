CREATE VIEW dbo.V_TradedCashReport        
AS        
SELECT     G.Symbol, G.OrderSideTagValue, CASE WHEN (OrderSideTagValue IN ('A', '1')) THEN 'Buy' WHEN (OrderSideTagValue IN ('B'))         
                      THEN 'BuyToCover' WHEN (OrderSideTagValue IN ('2', 'D')) THEN 'Sell' WHEN (OrderSideTagValue IN ('5', '6', 'C'))         
                      THEN 'SellShort' ELSE 'Unknown' END AS Side, CASE WHEN (ORderSideTagValue IN ('A', 'B', '1')) THEN 1 WHEN (ORderSideTagValue IN ('2', '5', '6',         
                      'C', 'D')) THEN - 1 ELSE 1 END AS SideMultiplier, FA.AllocatedQty, G.AvgPrice, ISNULL(L2A.Commission, 0) AS Commission,         
                      ISNULL(L2A.OtherBrokerFees, 0) AS Fees,         
                      ISNULL(ISNULL(L2A.StampDuty + L2A.TransactionLevy + L2A.ClearingFee + L2A.TaxOnCommissions + L2A.MiscFees + L2A.SecFee + L2A.OccFee + L2A.OrfFee, 0), 0) AS OtherFees,         
                      dbo.GetFormattedDatePart(G.AUECLocalDate) AS AUECLocalDate, dbo.GetFormattedDatePart(G.SettlementDate) AS settlementdate,         
                      Asset.AssetName AS Asset,ISNULL(SM.Multiplier,1)AssetMultiplier, dbo.T_UnderLying.UnderLyingName AS Underlying,         
                      SM.TickerSymbol AS AUEC_Currency, SM.VsCurrency AS VsCurrency, SM.LeadCurrency AS LeadCurrency,         
                      G.CurrencyID AS FXFromCurrencyID, G.AssetID, G.UnderLyingID,         
                      G.ExchangeID, SM.LeadCurrencyID, SM.VsCurrencyID, G.AUECID, AUEC.BaseCurrencyID AS AUEC_CurrencyID, FA.FundID, G.IsSwapped, CF.FundName,         
                      G.CurrencyID  AS CurrencyID,         
                      CASE WHEN G.AssetID = 5 THEN SM.VsCurrency ELSE SymbCurr.CurrencySymbol END AS SettlementCurrency,         
                      CASE G.IsSwapped WHEN 1 THEN 0 ELSE 1 END AS SwapMultiplier, G.FXRate AS TradeFXRate,         
                      G.FXConversionMethodOperator AS TradeFXConversionMethodOperator,ISNULL(L2A.ClearingBrokerFee, 0) AS ClearingBrokerFee, ISNULL(L2A.SoftCommission, 0) AS SoftCommission
FROM         dbo.T_Group AS G INNER JOIN        
                      dbo.T_FundAllocation AS FA ON FA.GroupID = G.GroupID INNER JOIN        
                      dbo.T_AUEC AS AUEC ON AUEC.AUECID = G.AUECID LEFT OUTER JOIN        
                      dbo.T_Currency AS SymbCurr ON G.CurrencyID = SymbCurr.CurrencyID INNER JOIN        
                      dbo.T_Level2Allocation AS L2A ON L2A.Level1AllocationID = FA.AllocationId INNER JOIN        
                      dbo.T_Asset AS Asset ON Asset.AssetID = G.AssetID INNER JOIN        
                      dbo.T_UnderLying ON dbo.T_UnderLying.UnderLyingID = G.UnderLyingID LEFT OUTER JOIN        
                      dbo.T_Side ON dbo.T_Side.SideTagValue = G.OrderSideTagValue INNER JOIN        
                      dbo.T_CompanyFunds AS CF ON FA.FundID = CF.CompanyFundID LEFT OUTER JOIN        
                      V_SecmasterData AS SM ON  SM.TickerSymbol  =G.Symbol       

