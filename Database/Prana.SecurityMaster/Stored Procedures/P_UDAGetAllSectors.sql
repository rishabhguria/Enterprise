CREATE procedure [dbo].[P_UDAGetAllSectors] as        
select   SectorName , SectorID        
from T_UDASector   
order by SectorName
