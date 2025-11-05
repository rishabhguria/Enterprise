CREATE procedure [dbo].[P_GetTTRiskValidateSettings]  
(  
  @companyUserID int    
)  
  
as  
select IsRiskChecked,IsValidateSymbolChecked,RiskValue,LimitPriceChecked,SetExecutedQtytoZero  
from T_TTRiskNValidationSetting  
where CompanyUserID = @companyUserID  

