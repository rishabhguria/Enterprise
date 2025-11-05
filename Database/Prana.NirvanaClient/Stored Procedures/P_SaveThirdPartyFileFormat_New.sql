CREATE PROCEDURE [dbo].[P_SaveThirdPartyFileFormat_New] (
	@thirdPartyId INT
	,@fileFormatId INT
	,@fileFormatName VARCHAR(50)
	,@nirvanaToThirdParty VARCHAR(200)
	,@headerFile VARCHAR(200)
	,@footerFile VARCHAR(200)
	,@nirvanaToThirdPartyData IMAGE
	,@headerFileData IMAGE
	,@footerFileData IMAGE
	,@fileType INT
	,@exportOnly BIT
	,@lastSaveTime DATETIME
	,@delimiter VARCHAR(5)
	,@delimiterName VARCHAR(20)
	,@fileExtension VARCHAR(20)
	,@fileDisplayName VARCHAR(50)
	,@SPName VARCHAR(50)
	,@DoNotShowFileOpenDialogue BIT
	,@ClearExternalTransID BIT
	,@IncludeExercisedAssignedTransaction BIT
	,@IncludeExercisedAssignedUnderlyingTransaction BIT
	,@IncludeCATransaction BIT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
 ,@GenerateCancelNewForAmend BIT=0  
	,@FIXEnabled BIT
    ,@FileEnabled BIT
    ,@FIXStorProc VARCHAR(50)
	,@TimeBatchEnabled BIT
	,@xmlTimeBatchesData NTEXT
	)
AS
DECLARE @result INT
DECLARE @total INT
DECLARE @nirvanaToThirdPartyID INT
DECLARE @headerFileID INT
DECLARE @footerFileID INT
DECLARE @handle INT
EXEC sp_xml_preparedocument @handle OUTPUT
	,@xmlTimeBatchesData

SET @total = 0
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRAN TRAN1

BEGIN TRY
	--Find out if the file format already there                          
	SELECT @total = Count(*)
	FROM T_ThirdPartyFileFormat
	WHERE FileFormatId = @fileFormatId
		AND ThirdPartyId = @thirdPartyId

	--Update if record exists                            
	IF (@total > 0)
	BEGIN
		-- Get all file ID's from T_ThirdPartyFileFormat table                          
		SELECT @nirvanaToThirdPartyID = NirvanaToThirdPartyID
			,@headerFileID = HeaderFileID
			,@footerFileID = FooterFileID
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatId
			AND ThirdPartyId = @thirdPartyId

		-- Update the data in the files if it exists in the T_FileData                          
		IF (@nirvanaToThirdPartyData IS NOT NULL)
		BEGIN
			IF (
					EXISTS (
						SELECT FileID
						FROM T_FileData
						WHERE FileID = @nirvanaToThirdPartyID
						)
					)
			BEGIN
				UPDATE T_FileData
				SET FileNames = @nirvanaToThirdParty
					,FileData = @nirvanaToThirdPartyData
					,LastSaveTime = @lastSaveTime
				WHERE FileId = @nirvanaToThirdPartyID
			END
			ELSE
			BEGIN
				INSERT T_FileData (
					FileNames
					,FileData
					,LastSaveTime
					,FileType
					)
				VALUES (
					@nirvanaToThirdParty
					,@nirvanaToThirdPartyData
					,@lastSaveTime
					,@fileType
					)

				SET @nirvanaToThirdPartyID = scope_identity()
			END
		END

		IF (@headerFileData IS NOT NULL)
		BEGIN
			IF (
					EXISTS (
						SELECT FileID
						FROM T_FileData
						WHERE FileID = @headerFileID
						)
					)
			BEGIN
				UPDATE T_FileData
				SET FileNames = @headerFile
					,FileData = @headerFileData
					,LastSaveTime = @lastSaveTime
				WHERE FileId = @headerFileID
			END
			ELSE
			BEGIN
				INSERT T_FileData (
					FileNames
					,FileData
					,LastSaveTime
					,FileType
					)
				VALUES (
					@headerFile
					,@headerFileData
					,@lastSaveTime
					,@fileType
					)

				SET @headerFileID = scope_identity()
			END
		END

		--If Footer File column was NULL earlier then INSERT else UPDATE                          
		IF (@footerFileData IS NOT NULL)
		BEGIN
			IF (
					EXISTS (
						SELECT FileID
						FROM T_FileData
						WHERE FileID = @footerFileID
						)
					)
			BEGIN
				UPDATE T_FileData
				SET FileNames = @footerFile
					,FileData = @footerFileData
					,LastSaveTime = @lastSaveTime
				WHERE FileId = @footerFileID
			END
			ELSE
			BEGIN
				INSERT T_FileData (
					FileNames
					,FileData
					,LastSaveTime
					,FileType
					)
				VALUES (
					@footerFile
					,@footerFileData
					,@lastSaveTime
					,@fileType
					)

				SET @footerFileID = scope_identity()
			END
		END

		--Finally UPDATE the existing file format entry in T_ThirdPartyFileFormat                          
		UPDATE T_ThirdPartyFileFormat
		SET FileFormatName = @fileFormatName
			,NirvanaToThirdPartyID = @nirvanaToThirdPartyID
			,HeaderFileID = @headerFileID
			,FooterFileID = @footerFileID
			,ExportOnly = @exportOnly
			,Delimiter = @delimiter
			,DelimiterName = @delimiterName
			,FileExtension = @fileExtension
			,FileDisplayName = @fileDisplayName
			,SPName = @SPName
			,DoNotShowFileOpenDialogue = @DoNotShowFileOpenDialogue
			,ClearExternalTransID = @ClearExternalTransID
			,IncludeExercisedAssignedTransaction = @IncludeExercisedAssignedTransaction
			,IncludeExercisedAssignedUnderlyingTransaction = @IncludeExercisedAssignedUnderlyingTransaction
			,IncludeCATransaction = @IncludeCATransaction
			,GenerateCancelNewForAmend = @GenerateCancelNewForAmend
			,FIXEnabled = @FIXEnabled
			,FileEnabled = @FileEnabled
			,FIXStorProc = @FIXStorProc
			,TimeBatchesEnabled = @TimeBatchEnabled
		WHERE FileFormatId = @fileFormatId
			AND ThirdPartyId = @thirdPartyId
		
	END

	--INSERT the file format if do not already exists in table                            
	IF (@fileFormatId < 1)
	BEGIN
		--First INSERT file data into the T_FileData and get the file ID's           
		IF (@nirvanaToThirdPartyData IS NOT NULL)
		BEGIN
			INSERT T_FileData (
				FileNames
				,FileData
				,LastSaveTime
				,FileType
				)
			VALUES (
				@nirvanaToThirdParty
				,@nirvanaToThirdPartyData
				,@lastSaveTime
				,@fileType
				)

			SET @nirvanaToThirdPartyID = scope_identity()

			SELECT @nirvanaToThirdPartyID
		END

		IF (@headerFileData IS NOT NULL)
		BEGIN
			INSERT T_FileData (
				FileNames
				,FileData
				,LastSaveTime
				,FileType
				)
			VALUES (
				@headerFile
				,@headerFileData
				,@lastSaveTime
				,@fileType
				)

			SET @headerFileID = scope_identity()

			SELECT @headerFileID
		END

		IF (@footerFileData IS NOT NULL)
		BEGIN
			INSERT T_FileData (
				FileNames
				,FileData
				,LastSaveTime
				,FileType
				)
			VALUES (
				@footerFile
				,@footerFileData
				,@lastSaveTime
				,@fileType
				)

			SET @footerFileID = scope_identity()
		END

		--Finally insert the data into the T_ThirdPartyFileFormat                          
		INSERT T_ThirdPartyFileFormat (
			FileFormatName
			,ThirdPartyId
			,NirvanaToThirdPartyID
			,HeaderFileID
			,FooterFileID
			,ExportOnly
			,Delimiter
			,DelimiterName
			,FileExtension
			,FileDisplayName
			,SPName
			,DoNotShowFileOpenDialogue
			,ClearExternalTransID
			,IncludeExercisedAssignedTransaction
			,IncludeExercisedAssignedUnderlyingTransaction
			,IncludeCATransaction
			,GenerateCancelNewForAmend
			,FIXEnabled
			,FileEnabled
			,FIXStorProc
			,TimeBatchesEnabled
			)
		VALUES (
			@fileFormatName
			,@thirdPartyId
			,@nirvanaToThirdPartyID
			,@headerFileID
			,@footerFileID
			,@exportOnly
			,@delimiter
			,@delimiterName
			,@fileExtension
			,@fileDisplayName
			,@SPName
			,@DoNotShowFileOpenDialogue
			,@ClearExternalTransID
			,@IncludeExercisedAssignedTransaction
			,@IncludeExercisedAssignedUnderlyingTransaction
			,@IncludeCATransaction
			,@GenerateCancelNewForAmend
			,@FIXEnabled
			,@FileEnabled
			,@FIXStorProc
			,@TimeBatchEnabled
			)
		SET @fileFormatId =  scope_identity();
	END

	-- Create a temporary table to store Time Batches data
	CREATE TABLE #TempTimeBatchesData (
	    colId INT,
	    colBatchRunTime TIME,
		colIsPaused BIT
	)
	
	-- Populate the temporary table with data from the parsed XML
	INSERT INTO #TempTimeBatchesData (colId, colBatchRunTime, colIsPaused)
	SELECT Id, BatchRunTime, IsPaused
	FROM openXML(@handle, 'DsTimeBatchesData/DtTimeBatchesData', 2) WITH (
	    Id INT,
	    BatchRunTime TIME,
		IsPaused BIT
	)
	
	-- Update T_ThirdPartyTimeBatches with selected BatchRunTime values
	UPDATE T_ThirdPartyTimeBatches
	SET BatchRunTime = colBatchRunTime,
		IsPaused = colIsPaused
	FROM T_ThirdPartyTimeBatches
	INNER JOIN #TempTimeBatchesData ON T_ThirdPartyTimeBatches.Id = #TempTimeBatchesData.colId

	-- Delete records from T_ThirdPartyTimeBatches
	DELETE FROM T_ThirdPartyTimeBatches
	WHERE FileFormatId = @fileFormatId
	  AND ID NOT IN (SELECT colId FROM #TempTimeBatchesData)
	
	-- Insert new details into T_ThirdPartyTimeBatches
	INSERT INTO T_ThirdPartyTimeBatches (FileFormatId, BatchRunTime, IsPaused)
	SELECT @fileFormatId, colBatchRunTime, colIsPaused
	FROM #TempTimeBatchesData
	WHERE colId < 1
	
	-- Clean up: drop the temporary table
	DROP TABLE #TempTimeBatchesData

	SELECT @result = FileFormatId
	FROM T_ThirdPartyFileFormat
	WHERE FileFormatId = @fileFormatId
		AND ThirdPartyId = @thirdPartyId

	SELECT @result

	COMMIT TRANSACTION TRAN1
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();

	ROLLBACK TRANSACTION TRAN1

	SELECT @ErrorNumber
END CATCH;
