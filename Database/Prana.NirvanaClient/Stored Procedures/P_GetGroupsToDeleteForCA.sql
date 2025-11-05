
/*    
P_GetGroupsToDeleteForCA '979E0AB6-6B61-47AB-9AF1-A72E35159089'        
*/
CREATE PROCEDURE P_GetGroupsToDeleteForCA (@caIds VARCHAR(max))
AS
BEGIN
	SELECT DISTINCT GroupId
	INTO #GroupIDTable
	FROM PM_Corpactiontaxlots caTaxlots
	INNER JOIN (
		SELECT Items
		FROM dbo.Split(@caIds, ',')
		) AS CaIds
		ON caTaxlots.CorpActionId = CaIds.Items
	WHERE ClosingId IS NULL

	SELECT VT.GroupID
		,VT.Symbol
		,VT.FundID
		,VT.Level2ID
		,VT.TaxlotQty
		,VT.AUECID
		,VT.AssetID
		,VT.OrderSideTagValue
		,VT.AUECLocalDate
	FROM V_Taxlots VT
	INNER JOIN #GroupIDTable
		ON VT.GroupID = #GroupIDTable.GroupID

	DROP TABLE #GroupIDTable
END
