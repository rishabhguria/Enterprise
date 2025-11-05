CREATE PROCEDURE P_CA_ArchiveAlertsBetweenDateRange 
	-- Add the parameters for the stored procedure here
	@startDate datetime, 
	@endDate datetime 
AS
BEGIN


-- Alerts in given date range will be archived INCLUDING BOTH DATES
-- this statement will archive all the alerts in given date range to archive table including both dates
/*
Date : 08-Aug-2021
Adding "ComplianceOfficerNotes".
Although Didn't find this SP's reference in either C# or in SQL.  
*/
insert into T_CA_ArchivedAlertHistory select UserId,RuleType,Summary,CompressionLevel,Parameters,OrderId,Status,ValidationTime,RuleId,Description,Dimension,GETDATE(),1,PreTradeType,ActionUser,PreTradeActionType,ComplianceOfficerNotes,UserNotes,TradeDetails from T_CA_AlertHistory as a where a.validationTime>=@startDate and a.validationTime<=@endDate

-- this statement will delete all the alerts in given date range from main alert history table including both dates
delete from T_CA_AlertHistory where validationTime>=@startDate and validationTime<=@endDate


END
