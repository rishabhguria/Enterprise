CREATE PROCEDURE [dbo].[P_CA_GetRuleOverRiddenPermission]
@userID int    
  
AS  
 SELECT DISTINCT
	RUD.RuleId as RuleId,	
	RUD.RuleName as RuleName,
	isnull(NRLP.RuleOverrideType,1) as AlertTypePermission,
	isnull(NRLP.ShowPopup,0) as PopUp,
	RUD.PackageName as PackageName
	
FROM 

T_CA_RulesUserDefined as RUD left outer join
T_CA_RuleUserPermissions as NRLP on RUD.RuleId=NRLP.RuleId and @userID=NRLP.UserId
where RUD.IsDeleted=0

order by RuleName

