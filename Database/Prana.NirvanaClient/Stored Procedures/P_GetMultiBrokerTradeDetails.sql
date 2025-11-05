CREATE PROCEDURE [dbo].[P_GetMultiBrokerTradeDetails]
AS
BEGIN
	SET NOCOUNT ON;;

	WITH LatestRecords
	AS (
		SELECT ParentCLOrderId
			,CounterPartyID
			,CLOrderID
			,ROW_NUMBER() OVER (
				PARTITION BY ParentCLOrderId
				,CounterPartyID ORDER BY InsertionTime DESC
				) AS rn
			,InsertionTime
		FROM T_MultiBrokerTradeDetails
		)
	SELECT ParentCLOrderId
		,CounterPartyID
		,CLOrderID
	FROM LatestRecords
	WHERE rn = 1
	ORDER BY InsertionTime DESC;
END