
CREATE PROCEDURE [dbo].[P_UDA_SaveDynamicUDA] 
	@Tag			VARCHAR(100),		
	@HeaderCaption	VARCHAR(100),
	@DefaultValue	VARCHAR(100),
	@MasterValues	XML,
	@RenamedKeys	VARCHAR(100)

AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

	IF(NULLIF(LTRIM(RTRIM(@RenamedKeys)),'') IS NOT NULL)
	BEGIN
		DECLARE	@Key	  INT
		DECLARE	@OldValue VARCHAR(100)
		DECLARE	@NewValue VARCHAR(100)
		CREATE TABLE #Temp(KeyValue INT)
		INSERT INTO #Temp SELECT * FROM Split(@RenamedKeys , ',')

		WHILE EXISTS(SELECT 1 FROM #Temp)
		BEGIN
			SELECT TOP 1 @Key = KeyValue FROM #Temp
			SELECT @OldValue =	REPLACE(MasterValues.value('(/MasterUDAValue/Value[@key = sql:variable("@Key")])[1]', 'varchar(100)'),'''','''''')
								FROM T_UDA_DynamicUDA
								WHERE Tag = @Tag
			SELECT @NewValue =	REPLACE(@MasterValues.value('(/MasterUDAValue/Value[@key = sql:variable("@Key")])[1]', 'varchar(100)'),'''','''''')
			IF(@NewValue IS NOT NULL)
			BEGIN
			EXEC P_UDA_RenameDynamicUDADataValues 
			@Tag = @Tag,
			@OldValue = @OldValue,
			@NewValue = @NewValue
			END

			DELETE FROM #Temp WHERE KeyValue = @Key
		END

		DROP TABLE #Temp
	END

	IF NOT EXISTS (SELECT 1 FROM dbo.T_UDA_DynamicUDA WHERE Tag = @Tag)
	BEGIN
		INSERT INTO T_UDA_DynamicUDA
			(
				Tag,
				HeaderCaption,
				DefaultValue,
				MasterValues
            )
		VALUES
           (
				@Tag,					
				@HeaderCaption,
				@DefaultValue,
				@MasterValues
			)
		
		EXEC('IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''T_UDA_DynamicUDAData'' AND COLUMN_NAME IN ('''+ @Tag + ''')) BEGIN ALTER TABLE T_UDA_DynamicUDAData ADD ' + @Tag + ' NVARCHAR(200) END')
	END
	ELSE
	BEGIN
		DECLARE	@ExistingDefaultValue	VARCHAR(100)
		SELECT	@ExistingDefaultValue = REPLACE(DefaultValue,'''','''''') FROM T_UDA_DynamicUDA WHERE Tag = @Tag

		IF((@ExistingDefaultValue IS NULL) OR (@DefaultValue <> @ExistingDefaultValue))
		BEGIN
			IF(@ExistingDefaultValue IS NULL)
				SET	@ExistingDefaultValue = 'Undefined'
			
			IF((@Tag <> 'Issuer') AND (@Tag <> 'RiskCurrency'))
			BEGIN
				EXEC('UPDATE	T_UDA_DynamicUDAData
					  SET		[' + @Tag + '] = ''' + @ExistingDefaultValue +'''
					  WHERE		[' + @Tag + '] IS NULL')
			END
	
			EXEC('UPDATE	T_FutureMultipliers
				  SET		DynamicUDA.modify(''insert <' + @Tag + '>' + @ExistingDefaultValue +'</' + @Tag + '> into (/DynamicUDAs)[1]'')
				  WHERE		DynamicUDA IS NOT NULL
							AND DynamicUDA.exist(''/DynamicUDAs/' + @Tag + ''') <> 1')

			EXEC('UPDATE	T_FutureMultipliers
				  SET		DynamicUDA = ''<DynamicUDAs> <' + @Tag + '>' + @ExistingDefaultValue +'</' + @Tag + '> </DynamicUDAs>''
				  WHERE		DynamicUDA IS NULL')
		END

		UPDATE T_UDA_DynamicUDA
		SET			
			HeaderCaption	= @HeaderCaption,
			DefaultValue	= @DefaultValue,
			MasterValues    = @MasterValues		
		WHERE Tag = @Tag
	END

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

