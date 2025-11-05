CREATE PROCEDURE [dbo].[P_GetManualFillsNew] (@ParentClOrderID VARCHAR(50))
AS
SELECT F.ExecutionID
	,F.Quantity
	,F.LastPx
	,F.LastShares
	,F.AveragePrice
	,F.TransactTime
	,F.ClOrderID
	,F.OrderStatus
	,O.AUECID
	,F.AveragePrice	
	,F.NotionalValue
	,F.NotionalValueBase
	,F.FxRate
	,F.FxRateCalc
	,F.SettlCurrency    
FROM T_Order AS O
JOIN T_Sub AS S ON O.ParentClOrderID = S.ParentClOrderID
JOIN T_Fills AS F ON S.ClOrderID = F.ClOrderID
WHERE O.ParentClOrderID = @ParentClOrderID
	AND F.LastShares > 0.0
ORDER BY F.ClOrderID

