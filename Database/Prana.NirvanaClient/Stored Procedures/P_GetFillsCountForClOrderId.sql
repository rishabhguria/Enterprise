CREATE PROCEDURE [dbo].[P_GetFillsCountForClOrderId]
(
	@ClOrderID varchar(50)
)
AS
	SELECT COUNT(*)
	FROM T_Fills
	Where ClOrderID = @ClOrderID