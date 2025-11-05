CREATE PROCEDURE [dbo].[P_UpdateBlotterClearanceTime] (
	@ClearanceTimeID INT
	,@ClearanceTime DATETIME
	,@CompanyAUECID INT
	,@PermitRollover BIT 
	,@IsSendManualOrderViaFIX BIT
	,@SendManualOrderTriggerTime DATETIME
	,@LastManualOrderRunTriggerTime DATETIME 
	)
AS
SET NOCOUNT ON

DECLARE @exists INT

SELECT @exists = COUNT(*)
FROM [T_CompanyAUECClearanceTimeBlotter]
WHERE [CompanyAUECID] = @CompanyAUECID

IF (@exists = 1)
BEGIN
	UPDATE [T_CompanyAUECClearanceTimeBlotter]
	SET ClearanceTime = @ClearanceTime, PermitRollover = @PermitRollover, IsSendManualOrderViaFIX = @IsSendManualOrderViaFIX, SendManualOrderTriggerTime = @SendManualOrderTriggerTime , LastManualOrderRunTriggerTime = @LastManualOrderRunTriggerTime
	WHERE [CompanyAUECID] = @CompanyAUECID
END
ELSE
BEGIN
	INSERT INTO [T_CompanyAUECClearanceTimeBlotter] (
		[CompanyAUECID]
		,ClearanceTime
		,PermitRollover
		,IsSendManualOrderViaFIX
		,SendManualOrderTriggerTime
		,LastManualOrderRunTriggerTime
		)
	VALUES (
		@CompanyAUECID
		,@ClearanceTime
		,@PermitRollover
		,@IsSendManualOrderViaFIX
		,@SendManualOrderTriggerTime
		,@LastManualOrderRunTriggerTime
		)
END