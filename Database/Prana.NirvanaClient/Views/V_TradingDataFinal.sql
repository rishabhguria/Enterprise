-- Modified By: Disha Sharma
-- Date: 12/09/2015
-- Description: Added READPAST while getting data from T_TradedOrders, T_Group and T_SwapParameters tables. It is used to avoid deadlock, PRANA-12033

CREATE VIEW [dbo].[V_TradingDataFinal]
AS  
SELECT DATEDIFF(d, GETDATE(), sub.AUECLocalDate) AS datedifference  
 ,sub.ClOrderID  
 ,sub.ParentClOrderID  
 ,sub.Price  
 ,dbo.T_Order.Symbol  
 ,fill.OrderID  
 ,sub.ExecutionInst  
 ,sub.OrderTypeTagValue AS OrderType  
 ,sub.TimeInForce  
 ,sub.CounterPartyID  
 ,sub.VenueID  
 ,sub.DiscrOffset  
 ,sub.PegDiff  
 ,sub.PNP  
 ,sub.StopPrice  
 ,sub.TargetCompID  
 ,sub.TargetSubID  
 ,sub.StagedOrderID  
 ,sub.NirvanaMsgType  
 ,sub.SecurityType  
 ,sub.OpenClose  
 ,CASE sub.MsgType
   WHEN 'G'    
    THEN ISNULL(fill.Quantity, sub.Quantity)
   ELSE sub.Quantity    
   END AS Quantity     
 ,sub.OrigClOrderID  
 ,ISNULL(fill.LastPx, 0) AS Lastpx  
 ,ISNULL(fill.AveragePrice, 0) AS AveragePrice  
 ,ISNULL(fill.LeavesQty, 0) AS LeavesQty  
 ,ISNULL(fill.CumQty, 0) AS CumQty  
 ,ISNULL(fill.OrderStatus, CASE sub.MsgType  
   WHEN 'F'  
    THEN '6'  
   WHEN 'G'  
    THEN 'E'  
   ELSE 'A'  
   END) AS OrderStatus  
 ,ISNULL(fill.LastShares, 0) AS LastShares  
 ,dbo.T_Order.ParentClOrderID AS Expr3  
 ,dbo.T_Order.AUECID  
 ,dbo.T_CompanyAUEC.CompanyAUECID  
 ,sub.UserID AS CompanyUserID  
 ,dbo.T_CompanyAUEC.AUECID AS Expr1  
 ,dbo.T_Order.TradingAccountID  
 ,sub.InsertionTime  
 ,dbo.V_OrderAUECDetails.CurrencyID  
 ,dbo.V_OrderAUECDetails.ExchangeID  
 ,dbo.V_OrderAUECDetails.AssetID  
 ,dbo.V_OrderAUECDetails.UnderLyingID  
 ,dbo.T_CountryFlag.CountryFlagImage  
 ,fill.NirvanaSeqNumber AS OrderSeqNumber  
 ,sub.ServerTime  
 ,dbo.T_CompanyUserAUEC.CompanyUserAUECID  
 ,ISNULL(fill.ExecutionID, '') AS Expr2  
 ,ISNULL(fill.MsgType, sub.MsgType) AS FillMsgType  
 ,sub.HandlingInst  
 ,dbo.T_Order.ListID  
 ,dbo.T_Order.OrderSidetagValue AS Side  
 ,sub.FundID  
 ,sub.StrategyID  
 ,sub.MsgType  
 ,sub.CMTA  
 ,sub.GiveUpID  
 ,sub.UnderlyingSymbol  
 ,sub.GiveUp  
 ,sub.AlgoStrategyID  
 ,sub.AlgoParameters  
 ,dbo.T_Order.OriginatorTypeID  
 ,sub.ClientOrderID  
 ,sub.AUECLocalDate  
 ,sub.SettlementDate  
 ,fill.SenderSubID  
 ,fill.AvgFxRateForTrade  
 ,sub.ProcessDate  
 ,dbo.T_Order.CalcBasis  
 ,dbo.T_Order.CommissionRate  
 ,fill.CommissionAmt  
 ,dbo.T_ImportFileLog.ImportFileName  
 ,dbo.T_ImportFileLog.ImportFileID  
 ,dbo.T_Order.SoftCommissionCalcBasis  
 ,dbo.T_Order.SoftCommissionRate  
 ,fill.SoftCommissionAmt  
 ,sub.TradeAttribute6  
 ,sub.TradeAttribute5  
 ,sub.TradeAttribute4  
 ,sub.TradeAttribute3  
 ,sub.TradeAttribute2  
 ,sub.TradeAttribute1  
 ,sub.InternalComments  
 ,sub.SettlCurrency  
 ,sub.FxRate  
 ,sub.FxRateCalc  
 ,G.IsSwapped    
 ,Swap.NotionalValue  
 ,Swap.BenchMarkRate  
 ,Swap.[Differential]  
 ,Swap.OrigCostBasis  
 ,Swap.DayCount  
 ,Swap.SwapDescription  
 ,Swap.FirstResetDate  
 ,Swap.OrigTransDate  
 ,Swap.ResetFrequency  
 ,Swap.ClosingPrice  
 ,Swap.ClosingDate  
 ,Swap.TransDate  
 ,Swap.ShouldOverrideNotional  
 ,Swap.ShouldOverrideCostBasis  
 ,G.ChangeType  
 ,TradedOrders.Text  
 ,TradedOrders.[OriginalAllocationPreferenceID]
 ,TradedOrders.TransactionSource
 ,G.StateID
 ,sub.IsHidden 
 ,SUBSTRING((SELECT ',' + CAST(FA.FundID AS VARCHAR)
     FROM T_FundAllocation FA  
     WHERE FA.GroupID = TradedOrders.GroupID  
     FOR XML PATH('')),2,200000) AS AccountIds
,SUBSTRING((SELECT ',' + CAST(L2A.Level2ID AS VARCHAR)    
     FROM T_Level2Allocation L2A      
     WHERE L2A.GroupID = TradedOrders.GroupID      
     FOR XML PATH('')),2,200000) AS StrategyIds
	 ,SUB.ExpireTime AS ExpireTime
	 ,fill.DayCumQty as DayCumQty
	 ,fill.DayAvgPx as DayAvgPx

FROM dbo.V_LatestFillByOrderSeqNumber  
INNER JOIN dbo.T_Fills AS fill ON dbo.V_LatestFillByOrderSeqNumber.OrderSeqNumber = fill.NirvanaSeqNumber  And fill.ClOrderID = V_LatestFillByOrderSeqNumber.ClOrderID 
RIGHT OUTER JOIN dbo.V_OrderAUECDetails  
INNER JOIN dbo.T_Order  
INNER JOIN dbo.T_Sub AS sub   
 ON dbo.T_Order.ParentClOrderID = sub.ParentClOrderID   
 ON dbo.V_OrderAUECDetails.ClOrderID = dbo.T_Order.ParentClOrderID   
INNER JOIN dbo.T_CompanyUserAUEC  
INNER JOIN dbo.T_CompanyAUEC   
 ON dbo.T_CompanyUserAUEC.CompanyAUECID = dbo.T_CompanyAUEC.CompanyAUECID   
 ON dbo.T_Order.AUECID = dbo.T_CompanyAUEC.AUECID  
 AND sub.UserID = dbo.T_CompanyUserAUEC.CompanyUserID   
LEFT OUTER JOIN dbo.T_CountryFlag   
 ON dbo.V_OrderAUECDetails.CountryFlagID = dbo.T_CountryFlag.CountryFlagID   
 ON fill.ClOrderID = sub.ClOrderID   
LEFT OUTER JOIN dbo.T_ImportFileLog ON dbo.T_Order.ImportFileID = dbo.T_ImportFileLog.ImportFileID   
LEFT OUTER JOIN dbo.T_TradedOrders AS TradedOrders WITH (READPAST) ON dbo.T_Order.ParentClOrderID = TradedOrders.ParentClOrderID 
LEFT OUTER JOIN dbo.T_Group AS G WITH (READPAST) ON G.GroupID = TradedOrders.GroupID
LEFT OUTER JOIN dbo.T_SwapParameters AS Swap WITH (READPAST) ON Swap.GroupID = G.GroupID 

