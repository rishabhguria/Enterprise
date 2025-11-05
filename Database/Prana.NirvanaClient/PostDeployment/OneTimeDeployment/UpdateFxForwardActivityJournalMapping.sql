declare @activityTypeId int
select  @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FxForward_Settled'
declare @AmountTypeIdForPnl int
declare @AmountTypeIdForFXPnl int
select @AmountTypeIdForPnl=AmountTypeId from T_ActivityAmountType where AmountType='PnL'
select @AmountTypeIdForFXPnl=AmountTypeId from T_ActivityAmountType where AmountType='FXPnL'
declare @creditAccount int


if exists(select * from T_ActivityJournalMapping where [ActivityTypeId_FK]=@activityTypeId and [AmountTypeId_FK]=@AmountTypeIdForPnl)
begin
select @creditAccount=SubAccountId from T_SubAccounts where acronym='FXForwardRealizedPNL'
update T_ActivityJournalMapping set CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForPnl
end

if exists(select * from T_ActivityJournalMapping where [ActivityTypeId_FK]=@activityTypeId and [AmountTypeId_FK]=@AmountTypeIdForFXPnl)
begin
select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
update T_ActivityJournalMapping set CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForFXPnl
end

