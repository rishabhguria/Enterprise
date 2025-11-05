
CREATE PROCEDURE [dbo].[P_UDA_GetDynamicUDAData]
	@FundID	INT
AS

BEGIN TRY

DECLARE @intErrorCode INT

BEGIN TRAN                                                 
                                                          
BEGIN                                                                               

	SELECT	Symbol_PK,	
			UDAData,
			FundID
	FROM	T_UDA_DynamicUDAData
	WHERE	FundID = @FundID

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
	
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );

END CATCH

