
CREATE PROCEDURE [dbo].[P_AL_GetAllocationCheckSidePref]
@CompanyId							INT
AS

BEGIN TRY
BEGIN TRAN

SELECT 
	 def.[CompanyId]
	,def.[CheckSidePreference]
FROM T_AL_AllocationDefaultRule AS def
where def.[CompanyId] =@CompanyId

COMMIT 
END TRY

BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
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