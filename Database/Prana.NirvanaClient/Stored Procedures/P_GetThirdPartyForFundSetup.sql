
/*        
Modified By: Ankit Gupta on 20 Oct, 2014
Description: Get only those third parties for fund setup, which are currently in active state.
*/        
CREATE procedure [dbo].[P_GetThirdPartyForFundSetup]
as
SELECT ThirdPartyID, ThirdPartyName
from T_ThirdParty inner JOIN T_ThirdPartyType 
on T_ThirdParty.ThirdPartyTypeID=T_ThirdPartyType.ThirdPartyTypeID
where T_ThirdPartyType.ThirdPartyTypeID=1 AND T_ThirdParty.isActive = 1  

