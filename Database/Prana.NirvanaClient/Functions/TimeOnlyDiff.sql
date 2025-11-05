
CREATE function [dbo].[TimeOnlyDiff](@DateTime1 DateTime, @DateTime2 DateTime)
-- returns only the time portion of a DateTime
returns int
as
begin 
 begin
if( (dbo.timeonly(@DateTime1) -dbo.timeonly(@DateTime2))  >='1900-01-01 00:00:00.000')
  
return 1
else 
return -1
end 
return 0
end