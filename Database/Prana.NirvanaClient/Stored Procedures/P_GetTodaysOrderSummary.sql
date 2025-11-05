-- =============================================                    
-- Author:  Ashish                    
-- Create date: 18th Sept 2006                    
-- Description: To get the complete summary (including fills) corresponding          
--to a particular ClOrderID                    
/*                  
exec P_GetTodaysOrderSummary        
          
*/
-- Modified By: Disha Sharma      
-- Date: 10/13/2015      
-- Description: Returned Currency ID and FX Rate Field                
-- =============================================                    
CREATE PROCEDURE [dbo].[P_GetTodaysOrderSummary]
AS
BEGIN
	SELECT DISTINCT Symbol
	INTO #TickerSymbols
	FROM T_Order O
	INNER JOIN T_Sub S ON O.ParentClOrderID = S.ParentClOrderID
	WHERE DATEDIFF(dd, cast(CONVERT(VARCHAR(12), O.InsertionTime) AS DATETIME), getutcdate()) <= 1

	SELECT *
	INTO #TempSecmaster
	FROM V_SecMasterData
	INNER JOIN #TickerSymbols ON V_SecMasterData.TickerSymbol = #TickerSymbols.Symbol

	Select GroupID,T_TradedOrders.Symbol Into #Temp
	from T_TradedOrders 
	INNER JOIN #TickerSymbols ON T_TradedOrders.Symbol = #TickerSymbols.Symbol
	WHERE DATEDIFF(dd, cast(CONVERT(VARCHAR(12), T_TradedOrders.InsertionTime) AS DATETIME), getutcdate()) <= 1
	
	Create Table #SwapParameters
	(
	 GroupID varchar(50) NULL,
	 Symbol varchar(50) NULL,
	 SwapParameters varchar(max)
	)

	Insert Into #SwapParameters
	Select T_SwapParameters.GroupID, #Temp.Symbol, Concat('abc~', NotionalValue, '^abc~', DayCount, '^abc~', BenchMarkRate, '^abc~', Differential, '^abc~', FirstResetDate, '^abc~', ResetFrequency, '^abc~', OrigTransDate, '^abc~', OrigCostBasis, '^abc~', SwapDescription, '^abc~', ClosingDate, '^abc~', ClosingPrice, '^abc~', TransDate)
	from T_SwapParameters
	Left Join #Temp On #Temp.GroupId = T_SwapParameters.GroupID 

	SELECT 1000000
		,-- to be removed                      
		clorderid
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
		,V_TradingData.Symbol
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
	FROM V_TradingData
	INNER JOIN #TickerSymbols ON V_TradingData.Symbol = #TickerSymbols.Symbol
	LEFT JOIN [T_CompanyAUECClearanceTimeBlotter] AS clearance ON V_TradingData.CompanyUserAUECID = clearance.[CompanyAUECID]
	INNER JOIN #TempSecmaster AS SecMasterData ON SecMasterData.TickerSymbol = V_TradingData.Symbol
	LEFT JOIN #SwapParameters Swap ON Swap.Symbol = V_TradingData.Symbol
	WHERE DATEDIFF(dd, cast(CONVERT(VARCHAR(12), AUECLocalDate) AS DATETIME), getutcdate()) <= 1
END