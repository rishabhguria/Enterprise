
CREATE Procedure [P_AL_RenameAllocationPreference]
(
	@PreferenceId int,
	@PreferenceName nvarchar(200)
)
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

--declare @PreferenceId int

-- Renaming Preference name in PreferenceDef table
Update T_AL_AllocationPreferenceDef
set 
[Name]=@PreferenceName
Where Id = @PreferenceId

COMMIT TRAN

Exec P_AL_GetAllocationPreferenceById @PreferenceId
END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK TRAN
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	

    -- Use RAISERROR inside the CATCH block to return 
    -- error information about the original error that 
    -- caused execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH
