
IF EXISTS(SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'T_TradeAudit ' AND COLUMN_NAME = 'AuditID')
begin
Update T_TradeAudit
set IsProcessed = 1
END