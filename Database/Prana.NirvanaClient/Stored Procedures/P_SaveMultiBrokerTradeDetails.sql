CREATE PROCEDURE [dbo].[P_SaveMultiBrokerTradeDetails]
	@parentClOrderId VARCHAR(50),
	@counterPartyId INT,
	@clOrderId VARCHAR(50)
AS
BEGIN
	INSERT INTO T_MultiBrokerTradeDetails
	SELECT @parentClOrderId
		,@counterPartyId
		,@clOrderId
		,GETDATE()
END
