


CREATE PROCEDURE dbo.P_GetAlertbyAlertTypeID
(
	@alertTypeID char
)
AS
--set @alertTypeID =1
	SELECT     AlertTypeID, AlertTypeName
FROM         T_RMAlertType
where  AlertTypeID = @alertTypeID




