CREATE PROCEDURE [dbo].[P_UpdateMultiBrokerTradeDetailsForCurrentDay]
(
    @ParentCLOrderId VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[T_MultiBrokerTradeDetails]
    WHERE ParentCLOrderId = @ParentCLOrderId;
END