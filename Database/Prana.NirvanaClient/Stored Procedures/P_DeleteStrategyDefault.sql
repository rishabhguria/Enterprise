




CREATE   procedure P_DeleteStrategyDefault
(

@UserID int

)
as

delete from  T_StrategyDefault

where CompanyUserID=@UserID






