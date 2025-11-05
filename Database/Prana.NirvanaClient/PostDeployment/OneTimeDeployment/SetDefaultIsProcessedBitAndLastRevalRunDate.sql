IF EXISTS(SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'T_TradeAudit ' AND COLUMN_NAME = 'AuditID')
begin
Update T_TradeAudit
set IsProcessed = 1
END

IF EXISTS(SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'T_LastCalcDateRevaluation ' AND COLUMN_NAME = 'LastRevalRunDate')
begin
Update 
T_LastCalcDateRevaluation
Set LastRevalRunDate = LastCalcDate
where LastRevalRunDate is NULL
End