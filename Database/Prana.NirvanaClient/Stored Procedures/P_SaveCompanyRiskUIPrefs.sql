

CREATE proc [dbo].[P_SaveCompanyRiskUIPrefs]  
(  
@companyID int,  
@MaxStressTestViewsWithVolSkew int,  
@MaxStressTestViewsWithoutVolSkew int 
 
  
)  
As
if((select count(*) from T_RiskUIPrefs where CompanyID = @companyID)= 0)  
begin  
  
insert into T_RiskUIPrefs  
(  
T_RiskUIPrefs.CompanyID,  
T_RiskUIPrefs.MaxStressTestViewsWithVolSkew,  
T_RiskUIPrefs.MaxStressTestViewsWithoutVolSkew 
)  
values  
(  
@companyID,  
@MaxStressTestViewsWithVolSkew ,  
@MaxStressTestViewsWithoutVolSkew  
)  
  
end  
else   
begin  
update T_RiskUIPrefs  
set  
MaxStressTestViewsWithVolSkew = @MaxStressTestViewsWithVolSkew,  
MaxStressTestViewsWithoutVolSkew = @MaxStressTestViewsWithoutVolSkew  
  
end

