SET IDENTITY_INSERT T_CA_NotifyFrequency ON;

delete from T_CA_NotifyFrequency

	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(1,1,		'As frequently as triggered(1 Min)',	'1 minutes',	'* * * * * '	);
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(2,10,	'No more than once every 10 minutes',	'10 minutes',	'0 0/10 * * * ?');
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(3,30,	'No more than once every 30 minutes',	'30 minutes',	'0 0/30 * * * ?');
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(4,60,	'No more than once per hour',			'60 minutes',	'0 0 * * * ?'	);
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(5,120,	'No more than once every 2 hours',		'2 hours',		'0 0 0/2 * * ?'	);
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(6,240,	'No more than once every 4 hours',		'4 hours',		'0 0 0/4 * * ?'	);
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(7,1440,	'No more than once per day',			'tomorrow',		'0 0 0 0 * ?'	);
	INSERT INTO T_CA_NotifyFrequency(Id, Minutes, MeasurementDescription, SnoozeDescription, CronExp)   Values(8,0,		'At specific times',					' ',			'0 0 0 0 0 0'	);
SET IDENTITY_INSERT T_CA_NotifyFrequency OFF;
