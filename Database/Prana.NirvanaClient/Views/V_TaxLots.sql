

CREATE VIEW [dbo].[V_TaxLots]
AS
SELECT L2.TaxLotID
	,G.Symbol
	,G.OrderSideTagValue
	,G.OrderTypeTagValue
	,G.CounterPartyID
	,G.VenueID
	,G.AUECID
	,G.TradingAccountID
	,L2.TaxLotQty
	,G.AvgPrice
	,G.UserID
	,L2.Level2Percentage
	,G.AllocationDate
	,G.AssetID
	,G.UnderLyingID
	,G.ExchangeID
	,G.CurrencyID
	,L1.FundID
	,L2.Level2ID
	,G.SettlementDate
	,G.Description
	,L2.Commission
	,G.AUECLocalDate
	,L1.GroupID
	,L2.Level1AllocationID
	,G.CumQty
	,G.Quantity
	,dbo.T_SwapParameters.NotionalValue
	,dbo.T_SwapParameters.BenchMarkRate
	,dbo.T_SwapParameters.Differential
	,dbo.T_SwapParameters.OrigCostBasis
	,dbo.T_SwapParameters.DayCount
	,dbo.T_SwapParameters.SwapDescription
	,dbo.T_SwapParameters.FirstResetDate
	,dbo.T_SwapParameters.OrigTransDate
	,G.IsSwapped
	,dbo.T_SwapParameters.TransDate
	,L2.OtherBrokerFees
	,L2.StampDuty
	,L2.TransactionLevy
	,L2.ClearingFee
	,L2.TaxOnCommissions
	,L2.MiscFees
	,L2.TaxLotState
	,G.GroupRefID
	,G.FXRate
	,CASE 
		WHEN (
				G.FXConversionMethodOperator = 'M'
				OR G.FXConversionMethodOperator = 'D'
				)
			THEN G.FXConversionMethodOperator
		ELSE 'M'
		END AS FXConversionMethodOperator
	,G.TaxlotClosingID_Fk
	,G.IsPreAllocated
	,G.AllocatedQty
	,L2.Commission + L2.OtherBrokerFees + L2.StampDuty + L2.TransactionLevy + L2.ClearingFee + L2.TaxOnCommissions + L2.MiscFees + L2.SecFee + L2.OccFee + L2.OrfFee + L2.ClearingBrokerFee + L2.SoftCommission + L2.OptionPremiumAdjustment AS TotalExpenses
	,CASE 
		WHEN (
				ORderSideTagValue IN (
					'A'
					,'B'
					,'1'
					)
				)
			THEN 1
		WHEN (
				ORderSideTagValue IN (
					'2'
					,'5'
					,'6'
					,'C'
					,'D'
					)
				)
			THEN - 1
		ELSE 1
		END AS SideMultiplier
	,G.ProcessDate
	,G.OriginalPurchaseDate
	,L2.AccruedInterest
	,L2.FXRate AS FXRate_Taxlot
	,CASE 
		WHEN (
				L2.FXConversionMethodOperator = 'M'
				OR L2.FXConversionMethodOperator = 'D'
				)
			THEN L2.FXConversionMethodOperator
		ELSE 'M'
		END AS FXConversionMethodOperator_Taxlot
	,L2.ExternalTransId
	,L2.LotId
	,L2.TradeAttribute1
	,L2.TradeAttribute2
	,L2.TradeAttribute3
	,L2.TradeAttribute4
	,L2.TradeAttribute5
	,L2.TradeAttribute6
	,G.TransactionType
	,L2.SecFee
	,L2.OccFee
	,L2.OrfFee
	,L2.ClearingBrokerFee
	,L2.SoftCommission
	,G.NirvanaProcessDate
	,G.TransactionSource
	,G.InternalComments
	,G.SettlCurrency AS SettlCurrency_Group
	,L2.SettlCurrency AS SettlCurrency_Taxlot
	,L2.OptionPremiumAdjustment AS OptionPremiumAdjustment
	,G.ChangeType
	,L2.AdditionalTradeAttributes
FROM dbo.T_Level2Allocation AS L2
INNER JOIN dbo.T_FundAllocation AS L1
	ON L2.TaxlotQty <> 0
	AND L2.Level1AllocationID = L1.AllocationId
INNER JOIN dbo.T_Group AS G
	ON G.GroupID = L1.GroupID
LEFT OUTER JOIN dbo.T_SwapParameters
	ON G.GroupID = dbo.T_SwapParameters.GroupID