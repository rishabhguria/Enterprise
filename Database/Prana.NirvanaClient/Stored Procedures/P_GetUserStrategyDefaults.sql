

CREATE procedure P_GetUserStrategyDefaults 
(
@UserID int
)
as

select  DefaultID,DefaultName,Strategies,StrategyValues from T_StrategyDefault
where CompanyUserID=@UserID


