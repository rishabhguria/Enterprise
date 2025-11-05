  
CREATE PROCEDURE [dbo].[P_GetAllCommissionRulesForCVAUEC]         
     
       
AS         
        
SELECT CVAUECRuleId,        
 CVId_FK,        
 AUECId_FK,       
 FundId_FK,      
 SingleRuleId_FK,      
 BasketRuleId_FK,      
 CompanyID ,  
 T_CounterPartyVenue.CounterPartyID ,  
 T_CounterPartyVenue.VenueID  
 FROM T_CommissionRulesForCVAUEC  left outer join     T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID = T_CommissionRulesForCVAUEC.CVId_FK  
