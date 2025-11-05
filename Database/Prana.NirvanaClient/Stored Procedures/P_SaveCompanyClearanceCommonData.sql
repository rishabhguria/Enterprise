CREATE PROCEDURE [dbo].[P_SaveCompanyClearanceCommonData] (
	@TimeZone VARCHAR(100)
	,@AutoClearing VARCHAR(10)
	,@BaseTime DATETIME
	,@RolloverPermittedUserID INT
	,@CompanyID INT
	,@IsSendRealtimeManualOrderViaFix INT
	)
AS
BEGIN
	IF (
			SELECT count(*)
			FROM [T_CompanyClearanceCommonData]
			) = 0
		INSERT INTO [T_CompanyClearanceCommonData] (
			TimeZone
			,AutoClearing
			,BaseTime
			,RolloverPermittedUserID
			,CompanyID
			,IsSendRealtimeManualOrderViaFix
			)
		VALUES (
			@TimeZone
			,@AutoClearing
			,@BaseTime
			,@RolloverPermittedUserID
			,@CompanyID
			,@IsSendRealtimeManualOrderViaFix
			)
	ELSE
		UPDATE [T_CompanyClearanceCommonData]
		SET TimeZone = @TimeZone
			,AutoClearing = @AutoClearing
			,BaseTime = @BaseTime
			,RolloverPermittedUserID = @RolloverPermittedUserID
			,CompanyID = @CompanyID
			,IsSendRealtimeManualOrderViaFix = @IsSendRealtimeManualOrderViaFix
END