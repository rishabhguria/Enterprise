

CREATE procedure [dbo].[P_GetExchnagesByUserID] (  
@userID int  
)  
as  
select distinct T_AUEC.ExchangeID,T_AUEC.FullNAme,T_Exchange.DisplayName   
from T_CompanyUserAUEC  
join T_CompanyAUEC   
on T_CompanyUserAUEC.CompanyAUECID=T_CompanyAUEC.CompanyAUECID  
join T_AUEC on  
T_CompanyAUEC.AUECID=T_AUEC.AUECID 
join T_Exchange
on  T_Exchange.ExchangeID= T_AUEC.ExchangeID
where T_CompanyUserAUEC.CompanyUserID=@userID
