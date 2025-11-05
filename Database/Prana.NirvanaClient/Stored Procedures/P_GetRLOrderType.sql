

CREATE PROCEDURE dbo.P_GetRLOrderType
(
		@AUECID int
)
AS
SELECT     DISTINCT T_OrderType.OrderTypesID, T_OrderType.OrderTypes
FROM         T_OrderType INNER JOIN
                      T_AUECOrderTypes ON T_OrderType.OrderTypesID = T_AUECOrderTypes.OrderTypeID
WHERE     (T_AUECOrderTypes.AUECID = @AUECID)
ORDER BY T_OrderType.OrderTypes

