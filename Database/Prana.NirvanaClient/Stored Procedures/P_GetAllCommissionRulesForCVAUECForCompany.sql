  
CREATE PROCEDURE [dbo].[P_GetAllCommissionRulesForCVAUECForCompany] (        
   @companyID  int        
)        
AS         
        
SELECT CVAUECRuleId,        
 CVId_FK,        
 AUECId_FK,       
 FundId_FK,      
 SingleRuleId_FK,      
 BasketRuleId_FK,      
 T_CommissionRulesForCVAUEC.CompanyID ,  
 T_CounterPartyVenue.CounterPartyID ,  
 T_CounterPartyVenue.VenueID  
 FROM T_CommissionRulesForCVAUEC  left outer join     T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID = T_CommissionRulesForCVAUEC.CVId_FK  
 Inner Join T_CompanyCounterPartyVenues on
 T_CompanyCounterPartyVenues.CounterPartyVenueID=T_CommissionRulesForCVAUEC.CVId_FK 
WHERE T_CompanyCounterPartyVenues.CompanyID = @companyID  