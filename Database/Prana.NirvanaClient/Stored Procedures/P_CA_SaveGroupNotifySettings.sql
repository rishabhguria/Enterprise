Create PROCEDURE [dbo].[P_CA_SaveGroupNotifySettings]
(
@GroupId varchar(50),
@GroupName varchar(100),
@PopUpEnabled bit,
@EmailEnabled bit,
@EmailCCList varchar(1000),
@EmailToList varchar(1000),
@LimitFrequencyMinutes int,
@AlertInTimeRange bit,
@StopAlertOnHolidays bit,
@StartTime datetime,
@EndTime datetime,
@SendInOneEmail BIT,
@Slot1 datetime,
@Slot2 datetime,
@Slot3 datetime,
@Slot4 datetime,
@Slot5 datetime,
@EmailSubject varchar(1000)
)
AS

IF NOT EXISTS (SELECT * FROM dbo.T_CA_RuleGroupSettings WHERE GroupId = @GroupId)
       INSERT INTO T_CA_RuleGroupSettings
           (	
			GroupId,		
			GroupName,
			PopUpEnabled,
			EmailEnabled,
			EmailCCList,
			EmailToList,
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
            )
     VALUES
           (
			@GroupId,		
			@GroupName,
			@PopUpEnabled,
			@EmailEnabled,
			@EmailCCList,
			@EmailToList,
			@EmailSubject,
			@LimitFrequencyMinutes,
			@AlertInTimeRange,
			@StopAlertOnHolidays,
			@StartTime,
			@EndTime,
			@SendInOneEmail,
			@Slot1,
			@Slot2,
			@Slot3,
			@Slot4,
			@Slot5
            )
    ELSE
update T_CA_RuleGroupSettings
       set

			GroupName=@GroupName,
			PopUpEnabled = @PopUpEnabled,
			EmailEnabled = @EmailEnabled,
			EmailCCList = @EmailCCList,
			EmailToList = @EmailToList,
			EmailSubject = @EmailSubject,
			LimitFrequencyMinutes = @LimitFrequencyMinutes,
			AlertInTimeRange=@AlertInTimeRange,
			StopAlertOnHolidays=@StopAlertOnHolidays,
			StartTime=@StartTime,
			EndTime=@EndTime,
			[SendInOneEmail] = @SendInOneEmail,
			Slot1 = @Slot1,
			Slot2 = @Slot2,
			Slot3 = @Slot3,
			Slot4 = @Slot4,
			Slot5 = @Slot5
           where GroupId=@GroupId
