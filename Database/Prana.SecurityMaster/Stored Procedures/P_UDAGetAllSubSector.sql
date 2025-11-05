CREATE  procedure [dbo].[P_UDAGetAllSubSector] as        
select SubSectorName , SubSectorID         
from T_UDASubSector   
order by SubSectorName
