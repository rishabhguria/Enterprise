create PROCEDURE [dbo].[P_CA_DeleteGroup] 
	-- Add the parameters for the stored procedure here
	@GroupId varchar(50)
	 
AS
BEGIN
	delete from T_CA_RuleGroupSettings where GroupId=@GroupId
END
