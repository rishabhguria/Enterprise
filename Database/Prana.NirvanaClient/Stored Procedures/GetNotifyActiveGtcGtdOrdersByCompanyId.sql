CREATE PROCEDURE [dbo].[GetNotifyActiveGtcGtdOrdersByCompanyId](
    @CompanyId INT
    )
AS
BEGIN
    SELECT 
        CompanyId,
        IsNotifyActiveGtcGtdOrders,
        TimeToNotify
    FROM 
        T_NotifyActiveGtcGtdOrders
    WHERE 
        CompanyId = @CompanyId;
END

