




CREATE  PROCEDURE [dbo].[P_GetManualFills]
(
	@ClOrderID varchar(50)
)
AS
	SELECT  
	 ExecutionID, 
	 Quantity, 
	 LastPx, 
	 LastShares, 
	 AveragePrice,
	 TransactTime,
ClOrderID,
OrderStatus

	
	FROM T_Fills
	Where ClOrderID = @ClOrderID




