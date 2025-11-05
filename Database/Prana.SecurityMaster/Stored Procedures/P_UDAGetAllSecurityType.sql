CREATE procedure [dbo].[P_UDAGetAllSecurityType] as        
select SecurityTypeName , SecurityTypeID         
from T_UDASecurityType  
order by SecurityTypeName
