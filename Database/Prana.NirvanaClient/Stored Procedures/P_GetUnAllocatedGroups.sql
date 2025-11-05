

CREATE  procedure [dbo].[P_GetUnAllocatedGroups]
(
 @allocationtype as varchar(10),
@currentDate as Datetime
)
as
select groupID from T_UnAllocatedGroup
where allocationtype=@allocationtype
and 
DATEDIFF(hh,DateGrouped,@currentDate) < 24


