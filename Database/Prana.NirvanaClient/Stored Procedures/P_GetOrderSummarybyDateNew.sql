-- =============================================
-- Author:		Ashish Poddar
-- Create date: 18th May, 2007
-- Description:	To get summary of orders by date in one single row. 
-- =============================================
CREATE PROCEDURE [dbo].[P_GetOrderSummarybyDateNew] @lowerdate DATETIME
	,@companyUserID INT
	,@upperDate DATETIME
AS
BEGIN
	CREATE TABLE #Temp1
		--Get clearance related data into the temporary table. 
		(
		[AUECID] INT NOT NULL
		,[CompanyID] INT NOT NULL
		,[OrigClearanceTime] DATETIME NULL
		,[LowerClearanceTime] DATETIME NOT NULL
		,[UpperClearanceTime] DATETIME NOT NULL
		)

	INSERT INTO #Temp1 (
		AUECID
		,CompanyID
		,OrigClearanceTime
		,LowerClearanceTime
		,UpperClearanceTime
		)
	EXEC [P_GetAUECandBlotterClearanceData] @lowerdate -- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.

	SET NOCOUNT ON;

	SELECT VTO.ParentClOrderID
		,VTO.OrderSideName
		,VTO.OrderSideTagValue
		,VTO.OrderTypeName
		,VTO.OrderTypeTagValue
		,VTO.CounterPartyID
		,VTO.CounterPartyName
		,VTO.VenueID
		,VTO.VenueName
		,VTO.Symbol
		,VTO.TradingAccountID
		,VTO.TradingAccountName
		,VTO.AvgPrice
		,VTO.Quantity
		,VTO.CumQty
		,VTO.Price
		,VTO.OrderStatus
		,VTO.TransactTime
		,VTO.AssetID
		,VTO.AssetName
		,VTO.UnderLyingID
		,VTO.UnderLyingName
	FROM V_TradedOrders AS VTO
	JOIN T_CompanyUser CU ON CU.UserID = VTO.OriginalUserID
	JOIN #Temp1 AS ClearanceTable ON CU.CompanyID = ClearanceTable.CompanyID
		AND VTO.AUECID = ClearanceTable.AUECID
	WHERE VTO.NirvanaMsgType != 3 -- Exclude Staged Orders
		AND VTO.InsertionTime > convert(VARCHAR, @lowerdate, 101) + ' 05:00:00 AM'
		AND VTO.InsertionTime < convert(VARCHAR, @upperDate, 101) + ' 05:00:00 AM'
		--and VTO.InsertionTime < convert(varchar,dateadd(d,1,@date),101) + ' 05:00:00 AM'
END

SELECT *
FROM V_TradedOrders