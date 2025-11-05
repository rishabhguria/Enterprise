-- Update isenabled column for enabling and disabling actions
update T_ActivityJournalMapping set IsEnabled=0 where 
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where acronym='FXL') or
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where acronym='FXL_CurrencySettled') or
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where acronym='FX_Settled') or
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where acronym='FXForward_Settled') or
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where Acronym='FXLongUnRealizedPNL') or
ActivityTypeId_FK=(select activityTypeId from T_ActivityType where Acronym='CashUnRealizedPNL')