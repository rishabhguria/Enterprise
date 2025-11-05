
CREATE PROCEDURE [dbo].[P_CA_GetAlertNotificationSettings]
AS
/* SELECT   PermissionID, PermissionName
FROM         T_Permission */

SELECT	Uuid,   
		RuleName,
		r.RuleID,
		PackageName,
		PopUpEnabled,
		EmailEnabled,
		EmailToList,
		EmailCCList,
		EmailSubject,
		LimitFrequencyMinutes,
		n.Minutes as Minutes,
		AlertInTimeRange,
		StopAlertOnHolidays,
		StartTime,
		EndTime,
		[SendInOneEmail],
		Slot1,
		Slot2,
		Slot3,
		Slot4,
		Slot5,
		GroupId,
		SUBSTRING((SELECT ',' + CAST(P.UserId AS VARCHAR)
					FROM	T_CA_RuleUserPermissions P
					WHERE	P.RuleId = r.RuleId AND P.ShowPopup = 1
					FOR XML PATH('')),2,200000) AS PopUpEnabledUsers
FROM	T_CA_RulesUserDefined r
		LEFT JOIN T_CA_NotifyFrequency n 
		ON r.LimitFrequencyMinutes = n.ID
WHERE	r.LimitFrequencyMinutes = n.ID 
		AND IsDeleted=0
