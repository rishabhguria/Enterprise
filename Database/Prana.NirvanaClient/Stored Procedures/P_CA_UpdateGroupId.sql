Create PROCEDURE [dbo].[P_CA_UpdateGroupId]
(
@RuleId varchar(50),
@GroupId varchar(50)
)
AS

IF EXISTS (SELECT * FROM dbo.T_CA_RulesUserDefined WHERE RuleID = @RuleId)
    
update T_CA_RulesUserDefined
       set
			GroupId=@GroupId		
			
           where RuleID=@RuleId
