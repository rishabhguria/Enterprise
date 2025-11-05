--------------------------------------------------------
--Created By: Bharat Raturi
--Date: 01/04/14
--Purpose: Get all the schedule types
--usage P_GetAllScheduleTypes
---------------------------------------------------------
Create procedure P_GetAllScheduleTypes
as
select ScheduleID, ScheduleName  from T_ScheduleTypes
