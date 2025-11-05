CREATE VIEW dbo.V_TradingData
AS
SELECT sub.ClOrderID
	, sub.ParentClOrderID
	, sub.Price
	, dbo.T_Order.Symbol
	, fill.OrderID
	, sub.ExecutionInst
	, sub.OrderTypeTagValue AS OrderType
	, sub.TimeInForce
	, sub.CounterPartyID
	, sub.VenueID
	, sub.DiscrOffset
	, sub.PegDiff
	, sub.PNP
	, sub.StopPrice
	, sub.TargetCompID
	, sub.TargetSubID
	, sub.StagedOrderID
	, sub.NirvanaMsgType
	, sub.SecurityType
	, sub.OpenClose
	, sub.Quantity
	, sub.OrigClOrderID
	, ISNULL(fill.LastPx, 0) AS Lastpx
	, ISNULL(fill.AveragePrice, 0) AS AveragePrice
	, ISNULL(fill.LeavesQty, 0) AS LeavesQty
	, ISNULL(fill.CumQty, 0) AS CumQty
	, ISNULL(fill.OrderStatus, 'A') AS OrderStatus
	, ISNULL(fill.LastShares, 0) AS LastShares
	, dbo.T_Order.ParentClOrderID AS Expr3
	, dbo.T_Order.AUECID
	, dbo.T_CompanyAUEC.CompanyAUECID
	, sub.UserID AS CompanyUserID
	, dbo.T_CompanyAUEC.AUECID AS Expr1
	, dbo.T_Order.TradingAccountID
	, sub.InsertionTime
	, dbo.V_OrderAUECDetails.CurrencyID
	, dbo.V_OrderAUECDetails.ExchangeID
	, dbo.V_OrderAUECDetails.AssetID
	, dbo.V_OrderAUECDetails.UnderLyingID
	, dbo.T_CountryFlag.CountryFlagImage
	, fill.NirvanaSeqNumber AS OrderSeqNumber
	, sub.ServerTime
	, dbo.T_CompanyUserAUEC.CompanyUserAUECID
	, ISNULL(fill.ExecutionID, '') AS Expr2
	, ISNULL(fill.MsgType, sub.MsgType) AS FillMsgType
	, sub.HandlingInst
	, dbo.T_Order.ListID
	, dbo.T_Order.OrderSidetagValue AS Side
	, sub.FundID
	, sub.StrategyID
	, sub.MsgType
	, sub.CMTA
	, sub.GiveUpID
	, sub.UnderlyingSymbol
	, sub.GiveUp
	, sub.AlgoStrategyID
	, sub.AlgoParameters
	, dbo.T_Order.OriginatorTypeID
	, sub.ClientOrderID
	, sub.AUECLocalDate
	, sub.SettlementDate
	, fill.SenderSubID
	, fill.AvgFxRateForTrade
	, sub.ProcessDate
	, sub.ChangeType
	, sub.SettlCurrency
	,sub.ExpireTime
	,dbo.T_Order.CalcBasis
	,dbo.T_Order.SoftCommissionCalcBasis
	,dbo.T_Order.CommissionRate
	,dbo.T_Order.SoftCommissionRate
FROM dbo.V_LatestFillByOrderSeqNumber
INNER JOIN dbo.T_Fills AS fill ON dbo.V_LatestFillByOrderSeqNumber.OrderSeqNumber = fill.NirvanaSeqNumber
RIGHT OUTER JOIN dbo.V_OrderAUECDetails
INNER JOIN dbo.T_Order
INNER JOIN dbo.T_Sub AS sub ON dbo.T_Order.ParentClOrderID = sub.ParentClOrderID ON dbo.V_OrderAUECDetails.ClOrderID = dbo.T_Order.ParentClOrderID INNER JOIN dbo.T_CompanyUserAUEC
INNER JOIN dbo.T_CompanyAUEC ON dbo.T_CompanyUserAUEC.CompanyAUECID = dbo.T_CompanyAUEC.CompanyAUECID ON dbo.T_Order.AUECID = dbo.T_CompanyAUEC.AUECID
AND sub.UserID = dbo.T_CompanyUserAUEC.CompanyUserID LEFT OUTER JOIN dbo.T_CountryFlag ON dbo.V_OrderAUECDetails.CountryFlagID = dbo.T_CountryFlag.CountryFlagID ON fill.ClOrderID = sub.ClOrderID