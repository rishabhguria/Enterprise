
CREATE PROCEDURE [dbo].[P_AL_SaveAllocationCheckSidePref] 
	@CompanyId							INT,	
	@CheckSidePreference			NVARCHAR(MAX)
	
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

		UPDATE T_AL_AllocationDefaultRule
		SET
			CheckSidePreference=@CheckSidePreference
           WHERE CompanyId = @CompanyId

COMMIT TRAN

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