CREATE PROCEDURE P_CA_GetGroupData 
	
AS
BEGIN
	SELECT
	GroupId,   
	GroupName,	
	PopUpEnabled,
	EmailEnabled,
	EmailToList,
	EmailCCList,
	EmailSubject,
	LimitFrequencyMinutes,
	AlertInTimeRange,
	StopAlertOnHolidays,
	StartTime,
	EndTime,
	[SendInOneEmail],
	Slot1,
	Slot2,
	Slot3,
	Slot4,
	Slot5
FROM    T_CA_RuleGroupSettings
END
