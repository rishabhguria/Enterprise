
--Modified : Rajat
-- Date  : 26 Nov 2007
-- Comments : Added one more comment - T_CompanyStrategy.StrategyName
CREATE procedure [dbo].[P_GetStrategiesByUserID] (  
@UserID int  
)  
as  
select T_CompanyStrategy.CompanyStrategyID,T_CompanyStrategy.StrategyShortName,T_CompanyStrategy.StrategyName    
 from T_CompanyUserStrategies,  
T_CompanyStrategy   
  
where   
T_CompanyStrategy.CompanyStrategyID=T_CompanyUserStrategies.CompanyStrategyID  
and T_CompanyUserStrategies.CompanyUserID=@UserID
order by T_CompanyStrategy.StrategyName
