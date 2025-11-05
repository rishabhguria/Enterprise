

CREATE PROCEDURE dbo.P_GetAlerts
(
	@alertTypeID char
)
AS
	SELECT     AlertTypeID, AlertTypeName
FROM         T_RMAlertType



