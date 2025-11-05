
CREATE PROCEDURE [dbo].[P_CA_SaveNotifySettings]
(
@Uuid varchar(50),
@RuleName varchar(100),
@RuleId varchar(50),
@PackageName varchar(100),
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
@GroupId varchar(50),
@EmailSubject varchar(1000)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION  Tran1
		IF NOT EXISTS (SELECT * FROM dbo.T_CA_RulesUserDefined WHERE RuleID = @RuleId)
		BEGIN
			IF EXISTS (SELECT * FROM dbo.T_CA_RulesUserDefined WHERE RuleName= @RuleName AND IsDeleted=0 AND PackageName=@PackageName)
			BEGIN
			 Select RuleID AS OldRuleID,@RuleID as NewRuleID, RuleName into #Temp from T_CA_RulesUserDefined  WHERE RuleName=@RuleName AND IsDeleted=0 AND PackageName=@PackageName

				UPDATE	R
				SET		
						R.RuleID=T.NewRuleID
						From T_CA_RuleUserPermissions R Inner Join #Temp T  On T.OldRuleID=R.RuleId

				UPDATE	T_CA_RulesUserDefined
				SET		
						RuleID=@RuleId,
						Uuid=@Uuid			
				WHERE	RuleName=@RuleName AND IsDeleted=0 AND PackageName=@PackageName
		
					Drop table #Temp
			END
		ELSE
		BEGIN
	
			INSERT INTO T_CA_RulesUserDefined
				(
				Uuid,
				RuleName,
				RuleID,
				PackageName,
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
				Slot5,
				GroupId			
				)
			VALUES
				(
				@Uuid,
				@RuleName,
				@RuleId,
				@PackageName,
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
				@Slot5,
				@GroupId
				)
			
				IF(@PackageName = 'PreTrade')
				BEGIN
					INSERT INTO T_CA_RuleUserPermissions (RuleId, UserId, ShowPopup, RuleOverrideType)
					select @RuleId, OCP.UserId, OCP.DefaultPrePopUp, OCP.DefaultRuleOverrideType From T_CA_OtherCompliancePermission OCP
				END
				IF(@PackageName = 'PostTrade')
				BEGIN
					INSERT INTO T_CA_RuleUserPermissions (RuleId, UserId, ShowPopup, RuleOverrideType)
					select @RuleId, OCP.UserId, OCP.DefaultPostPopUp, OCP.DefaultRuleOverrideType From T_CA_OtherCompliancePermission OCP
				END

		END
		END
		ELSE
		BEGIN
			UPDATE	T_CA_RulesUserDefined
			SET		RuleName=@RuleName,
					Uuid=@Uuid,
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
					Slot5 = @Slot5,
					GroupId=@GroupId			
			WHERE	RuleID=@RuleId
		END
		  COMMIT TRANSACTION Tran1
		END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION Tran1
	END CATCH
END