
create view V_AUECTimingsWithClerance
as

select AUECID,convert(varchar(8),RegularTradingStartTime,108) as ClearanceStartTime,
convert(varchar(8),DATEADD(hh,5,RegularTradingEndTime),108) as ClearanceEndTime
from T_AUEC


