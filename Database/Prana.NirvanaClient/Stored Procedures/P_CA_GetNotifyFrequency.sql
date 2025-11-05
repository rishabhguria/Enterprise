
CREATE PROCEDURE [dbo].[P_CA_GetNotifyFrequency] 
	
AS
BEGIN
	select ID,MeasurementDescription,Minutes  from T_CA_NotifyFrequency
END
