
CREATE PROCEDURE [dbo].[P_AL_DeleteCostAdjustmentTaxlots]
(
	@CAIDs VARCHAR(MAX)
)
AS

BEGIN TRY

DECLARE @intErrorCode INT

BEGIN TRAN                                                 
                                                          
BEGIN                                                                               

	DELETE FROM T_AL_CostAdjustedTaxlots WHERE CostAdjustmentID IN (@CAIDs)

END 

COMMIT 

END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
    DECLARE @ErrorMessage	NVARCHAR(4000);
    DECLARE @ErrorSeverity	INT;
    DECLARE @ErrorState		INT;

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

