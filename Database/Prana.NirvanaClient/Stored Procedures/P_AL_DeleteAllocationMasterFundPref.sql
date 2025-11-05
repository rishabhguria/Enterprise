CREATE PROCEDURE [dbo].[P_AL_DeleteAllocationMasterFundPref]
(
	@MasterFundPrefId int
)
AS

BEGIN TRY

DECLARE @intErrorCode INT
DECLARE @calculatedPrefId INT
BEGIN TRAN

-- Getting all the calculated pref Id's associated with Master fund Id and stored in Temp Table
CREATE TABLE #CalculatedPrefIDs (CalculatedPrefId INT)   
	INSERT INTO #CalculatedPrefIDs   
	SELECT CalculatedPrefId from T_AL_MFWisePrefValues 
	WHERE MFPreferenceId = @MasterFundPrefId

-- Calling the SP 'P_AL_DeleteAllocationPreference' in Loop for all the calculated pref Id's
WHILE EXISTS (Select 1 from #CalculatedPrefIDs)
BEGIN
    SELECT TOP 1 @calculatedPrefId = [CalculatedPrefId] from #CalculatedPrefIDs
	Exec P_AL_DeleteAllocationPreference @calculatedPrefId
	Delete FROM #CalculatedPrefIDs WHERE CalculatedPrefId = @calculatedPrefId
END

-- Deleting from T_AL_MFWisePrefValues table
DELETE FROM T_AL_MFWisePrefValues
WHERE MFPreferenceId = @MasterFundPrefId

-- Deleting from T_AL_MasterFundProrataList table
DELETE FROM T_AL_MasterFundProrataList
WHERE MFPreferenceId = @MasterFundPrefId

-- Deleting from T_AL_MFAllocationPreference table
DELETE FROM T_AL_MFAllocationPreference
WHERE MFPreferenceId = @MasterFundPrefId

DROP TABLE #CalculatedPrefIDs  

COMMIT
END TRY
BEGIN CATCH
	ROLLBACK
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	
	RAISERROR (
			   @ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State
			   );
END CATCH


