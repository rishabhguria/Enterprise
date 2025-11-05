CREATE PROCEDURE [dbo].[P_SaveThirdPartyPBMismatchOverrideAudit]
	@UserId INT,
    @Time DATETIME
AS
BEGIN
    INSERT INTO [T_ThirdPartyPBMismatchOverrideAudit] (UserId, [Time])
    VALUES (@UserId, @Time);
END
