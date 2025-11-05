SET IDENTITY_INSERT T_ScheduleTypes ON;
delete from T_ScheduleTypes

		INSERT INTO T_ScheduleTypes(ScheduleID, ScheduleName) Values(0,'One time');
		INSERT INTO T_ScheduleTypes(ScheduleID, ScheduleName) Values(1,'Daily');
		INSERT INTO T_ScheduleTypes(ScheduleID, ScheduleName) Values(2,'Weekly');
		INSERT INTO T_ScheduleTypes(ScheduleID, ScheduleName) Values(3,'Monthly');


SET IDENTITY_INSERT T_ScheduleTypes OFF