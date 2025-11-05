

CREATE PROCEDURE  P_GetRulesByUserID
@UserID int , @RuleTypeID int

as

select distinct UR.RuleID,R.[Rule],UR.Checked from 
T_Rules join T_RuleType
on T_Rules.RuleTypeID=T_RuleType.RuleTypeID,

T_UserRule as UR join T_Rules as R on
UR.RuleId=R.RuleId
where UR.UserID=@UserID 
and T_RuleType.RuleTypeID=@RuleTypeID



