
CREATE PROCEDURE [dbo].[P_UDA_RenameDynamicUDADataValues] 
	@Tag		VARCHAR(100),	
	@OldValue	VARCHAR(500),
	@NewValue	VARCHAR(100)
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN
--	UPDATE	T_UDA_DynamicUDAData
--	SET		UDAData.modify('replace value of (/DynamicUDAs/*[local-name()=sql:variable("@Tag")]/text())[1] with sql:variable("@NewValue")')
--	WHERE	UDAData.value('(/DynamicUDAs/*[local-name()=sql:variable("@Tag")]/text())[1]','VARCHAR(100)') = @OldValue

	EXEC  ('UPDATE	T_UDA_DynamicUDAData
			SET		[' + @Tag + '] = ''' + @NewValue +'''
			WHERE	[' + @Tag + '] = ''' + @OldValue + '''')

	UPDATE	T_FutureMultipliers
	SET		DynamicUDA.modify('replace value of (/DynamicUDAs/*[local-name()=sql:variable("@Tag")]/text())[1] with sql:variable("@NewValue")')
	WHERE	DynamicUDA.value('(/DynamicUDAs/*[local-name()=sql:variable("@Tag")]/text())[1]','VARCHAR(100)') = @OldValue
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
	

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH

