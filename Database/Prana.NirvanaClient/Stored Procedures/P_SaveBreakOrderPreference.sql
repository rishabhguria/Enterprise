CREATE PROCEDURE [dbo].[P_SaveBreakOrderPreference]
	@companyId int,
	@prefernceValue bit
AS
	IF EXISTS (SELECT * FROM T_BreakOrderBasedOnPreference WHERE CompanyID =@companyId) 
	BEGIN
		UPDATE T_BreakOrderBasedOnPreference
		set IsBreakOrderEnabled=@prefernceValue
		where CompanyID=@companyId
	END
	ELSE
	BEGIN
		INSERT INTO T_BreakOrderBasedOnPreference VALUES(@companyId,@prefernceValue)
	END
