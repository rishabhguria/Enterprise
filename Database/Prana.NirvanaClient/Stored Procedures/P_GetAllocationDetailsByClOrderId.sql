CREATE PROCEDURE [dbo].[P_GetAllocationDetailsByClOrderID]
	 @parentClOrderID varchar(50)
AS
CREATE TABLE #ParentClOrderIDs (ParentClOrderID VARCHAR(50))   
	INSERT INTO #ParentClOrderIDs   
	SELECT * FROM dbo.Split((@parentClOrderID),',')   

	
CREATE TABLE #GroupIDs (GroupID VARCHAR(50))   
	INSERT INTO #GroupIDs   
	SELECT GroupID FROM T_TradedOrders T with (nolock) where T.ParentClOrderID IN (SELECT ParentClOrderID FROM #ParentClOrderIDs) 

	SELECT DISTINCT  F.FundID, 
	ROUND(F.Percentage,4) as Percentage, ROUND(F.AllocatedQty, 4) as AllocatedQty,T.GroupID, G.Quantity,T.ParentClOrderID, T.Quantity as OrderQuantity, T.CumQty, 
	G.AvgPrice as AvgPrice, G.FXRate as FXRate, G.FXConversionMethodOperator as FXConversionMethodOperator, G.assetId as AssetID, G.CurrencyID as CurrencyID   
	FROM T_TradedOrders T with (nolock)
	Left JOIN T_FundAllocation F
    ON T.GroupID = F.GroupID
	INNER JOIN #GroupIDs GI
    ON GI.GroupID = T.GroupID
	INNER JOIN T_Group G with (nolock)
    ON G.GroupID = T.GroupID 

DROP TABLE #ParentClOrderIDs,#GroupIDs