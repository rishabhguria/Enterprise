CREATE PROCEDURE [dbo].[UpdateInsertNotifyActiveGtcGtdOrders](
    @CompanyId INT,
    @IsActive INT,
    @NotifyTime DATETIME
    )
AS
BEGIN
    -- Check if the CompanyId already exists
    IF EXISTS (SELECT * FROM T_NotifyActiveGtcGtdOrders WHERE CompanyId = @CompanyId)
    BEGIN
        -- Update 
        UPDATE [T_NotifyActiveGtcGtdOrders]
        SET
            IsNotifyActiveGtcGtdOrders = @IsActive,
            TimeToNotify = @NotifyTime
        WHERE
            CompanyId = @CompanyId;
    END
    ELSE
    BEGIN
        -- Insert 
         INSERT INTO [T_NotifyActiveGtcGtdOrders](
            CompanyId,
            TimeToNotify,
            IsNotifyActiveGtcGtdOrders
           )
         VALUES (
            @CompanyId,
            @NotifyTime,
            @IsActive
          );
    END
END


