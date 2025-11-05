/***************************************
Author: Rahul Gupta
Creation Date: Sep'09,2012

This is used for getting the permitted funds to all the prime brokers of the company

****************************************/

CREATE PROCEDURE P_GetFundPBDetails  
AS  
SELECT   
Mapping.InternalFundnameID_Fk as FundID,  
TP.ThirdPartyName as PBName
from T_CompanyThirdPartyMappingDetails Mapping  
inner join T_CompanyThirdparty CTP on Mapping.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID  
inner join T_ThirdParty TP on TP.ThirdPartyID = CTP.ThirdPartyID