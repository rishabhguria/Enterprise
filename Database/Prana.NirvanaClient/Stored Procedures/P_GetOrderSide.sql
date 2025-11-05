

CREATE PROCEDURE dbo.P_GetOrderSide
(
	@userID int
)
AS
	SELECT OrderSideID, OrderSide
	FROM T_OrderSide
	ORDER BY OrderSideID
	


