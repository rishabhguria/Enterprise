-- =============================================              
-- Author:  Ashish              
-- Create date: 18th Sept 2006              
-- Description: To get the complete summary (including fills) corresponding    
--to a particular ClOrderID              
/*            
exec P_GetOrderSummaryByOrderID @ClOrderID='2012101003434142'    
    
*/
-- Modified By: Disha Sharma
-- Date: 10/13/2015
-- Description: Returned Currency ID and FX Rate Field          
-- =============================================              
CREATE PROCEDURE [dbo].[P_GetOrderSummaryByOrderID] @ClOrderID VARCHAR(50)
AS
BEGIN
	DECLARE @Symbol VARCHAR(500)
	SET @Symbol = (
			SELECT Symbol
    FROM T_Order O  With (NoLock)   
    INNER JOIN T_Sub S  With (NoLock) ON O.ParentClOrderID = S.ParentClOrderID    
			WHERE S.ClOrderID = @ClOrderID
			)

	SELECT *
	INTO #TempSecmaster
    FROM V_SecMasterData With (NoLock)   
	WHERE TickerSymbol = @Symbol

	DECLARE @GroupID VARCHAR(100)
	SET @GroupID = (
			SELECT GroupID
    FROM T_TradedOrders With (NoLock)   
			WHERE T_TradedOrders.ClOrderID = @ClOrderID
			)

	Create Table #SwapParameters
	(
	GroupID varchar(50) NULL,
	ClOrderID varchar(50) NULL,
	SwapParameters varchar(max)
	)

	Insert Into #SwapParameters
	Select @GroupID, @ClOrderID, Concat('abc~', NotionalValue, '^abc~', DayCount, '^abc~', BenchMarkRate, '^abc~', Differential, '^abc~', FirstResetDate, '^abc~', ResetFrequency,
	'^abc~', OrigTransDate, '^abc~', OrigCostBasis, '^abc~', SwapDescription, '^abc~', ClosingDate, '^abc~', ClosingPrice, '^abc~', TransDate)
	from T_SwapParameters
	where GroupID= @GroupID

	SELECT 1000000
		,-- to be removed                
		V_TradingData.clorderid
		,ParentClorderiD
		,'Issuborder'
		,--to be removed                
		lastpx
		,averagePrice
		,'IsStaged'
		,--to be removed                
		LeavesQty
		,CumQty
		,OrderStatus
		,LastShares
		,Quantity
		,Symbol
		,Side
		,OrderType
		,Price
		,'TA'
		,--TradingAccountName,  to be removed                
		OrigClorderiD
		,'IsManual'
		,--to be removed                
		ServerTime
		,CounterPartyID
		,VenueID
		,V_TradingData.AUECID
		,SecMasterData.AssetID
		,UnderLyingID
		,CountryFlagImage
		,StagedOrderID
		,TradingAccountID
		,CompanyUserID
		,NirvanaMsgType
		,DiscrOffset
		,PegDiff
		,StopPrice
		,ClearanceTime
		,SecMasterData.ExpirationDate
		,SecMasterData.StrikePrice
		,SecMasterData.PutOrCall
		,SecurityType
		,OpenClose
		,OrderSeqNumber
		,FundID
		,StrategyID
		,AUECLocalDate
		,SettlementDate
		,ProcessDate
		,ChangeType
		,SettlCurrency
		,SecMasterData.CurrencyID
		,V_TradingData.AvgFxRateForTrade
        ,Swap.SwapParameters
		,TimeInForce
		,ExchangeID
		,HandlingInst
		,ExpireTime
		,CalcBasis
		,SoftCommissionCalcBasis
		,CommissionRate
		,SoftCommissionRate
		,SecMasterData.RoundLot
 FROM V_TradingData  With (NoLock)  
 LEFT JOIN [T_CompanyAUECClearanceTimeBlotter] AS clearance  With (NoLock) ON V_TradingData.CompanyAUECID = clearance.[CompanyAUECID]    
	INNER JOIN #TempSecmaster AS SecMasterData ON SecMasterData.TickerSymbol = V_TradingData.Symbol
		AND V_TradingData.Symbol = @Symbol
	LEFT JOIN #SwapParameters Swap ON Swap.ClOrderID = V_TradingData.ClOrderID
	WHERE V_TradingData.ClOrderID = @ClOrderID
END