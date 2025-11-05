CREATE PROCEDURE [dbo].[P_GetCompanyClearanceCommonData]
(
@CompanyID INT
)
AS
BEGIN
	SELECT TimeZone
		,AutoClearing
		,BaseTime
		,RolloverPermittedUserID
		,IsSendRealtimeManualOrderViaFix
	FROM [T_CompanyClearanceCommonData]
	WHERE CompanyID = @CompanyID
END