CREATE PROCEDURE [dbo].[P_DeleteThirdPartyToleranceProfile] 
(
    @ToleranceProfileID INT
)
AS
BEGIN
    DELETE FROM T_ThirdPartyToleranceProfile
    WHERE ThirdPartyToleranceProfileId = @ToleranceProfileID
END
GO
