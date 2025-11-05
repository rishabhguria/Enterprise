

CREATE proc [dbo].[P_GetCompanyRiskUIPrefs]    
(  
@companyID int  
  
)  
as


select T_RiskUIPrefs.MaxStressTestViewsWithVolSkew, T_RiskUIPrefs.MaxStressTestViewsWithoutVolSkew 
from T_RiskUIPrefs where T_RiskUIPrefs.CompanyID = @companyID

