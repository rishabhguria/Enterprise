CREATE PROCEDURE [dbo].[P_AL_RenameAllocationMasterFundPref]
(
	@MfPreferenceId int,
	@MfPreferenceName nvarchar(200)
)
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

-- Renaming Preference name in T_AL_MFAllocationPreference table
Update T_AL_MFAllocationPreference
set 
[MFPreferenceName]=@MfPreferenceName
Where MFPreferenceId = @MfPreferenceId

Exec P_AL_GetMasterFundPrefById @MfPreferenceId

COMMIT TRAN

END TRY

BEGIN CATCH
	ROLLBACK TRAN
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	
    RAISERROR (
			   @ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH

