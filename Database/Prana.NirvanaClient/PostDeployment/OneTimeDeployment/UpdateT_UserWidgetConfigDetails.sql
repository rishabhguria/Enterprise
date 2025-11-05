--Add WidgetId for all the widgets where widget key is empty string

IF EXISTS(SELECT *	FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_RTPNL_UserWidgetConfigDetails' AND COLUMN_NAME = 'WidgetId')
BEGIN
     UPDATE T_RTPNL_UserWidgetConfigDetails 
     SET WidgetId = NEWID()
     WHERE WidgetId = ''
END

