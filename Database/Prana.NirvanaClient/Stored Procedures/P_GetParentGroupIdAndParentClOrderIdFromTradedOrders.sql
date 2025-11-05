CREATE PROCEDURE [dbo].[P_GetParentGroupIdAndParentClOrderIdFromTradedOrders]
AS
BEGIN
	SELECT GroupID, ParentClOrderID from  T_TradedOrders
END