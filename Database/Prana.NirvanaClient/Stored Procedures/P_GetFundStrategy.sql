


CREATE  procedure  P_GetFundStrategy

as
select isnull(T_CompanyStrategy.CompanyStrategyID,0) as CompanyStrategyID
,isNull(T_CompanyStrategy.StrategyName,0) 
as StrategyName,
T_Temp.CompanyFundID,T_Temp.FundName from 
(select FundName,StrategyID,T_CompanyFunds.CompanyFundID  from T_FundStrategy

right join T_CompanyFunds 
on
T_CompanyFunds.CompanyFundID=T_FundStrategy.FundID)
as T_Temp 
left join T_CompanyStrategy on
 T_Temp.StrategyID =T_CompanyStrategy.CompanyStrategyID




