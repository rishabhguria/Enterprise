
create PROCEDURE [dbo].[P_CA_DeleteRule] 
	-- Add the parameters for the stored procedure here
	@RuleId [varchar](50)
	 
AS
BEGIN
	--delete from T_CA_RulesUserDefined where RuleId=@RuleId
	update T_CA_RulesUserDefined set IsDeleted='TRUE' where RuleId=@RuleId
	
END


