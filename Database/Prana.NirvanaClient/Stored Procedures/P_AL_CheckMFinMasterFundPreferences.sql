CREATE PROCEDURE [dbo].[P_AL_CheckMFinMasterFundPreferences]
(
@companyID int,
@MFID int
)
AS

BEGIN TRY
	
BEGIN TRAN
	
	CREATE TABLE #Temp_Id
	(
		MasterFundId int
	)
	INSERT INTO #Temp_Id
	SELECT MFPrefVal.MFId FROM T_AL_MFAllocationPreference AS MFPref JOIN T_AL_MFWisePrefValues AS MFPrefVal
	ON MFPref.MFPreferenceId = MFPrefVal.MFPreferenceId AND MFPref.CompanyId= @companyID AND MFPrefVal.MFId= @MFID

	INSERT INTO #Temp_Id
	SELECT PreferencedFundId FROM T_AL_MFAllocationPreference
	WHERE CompanyId= @companyID AND PreferencedFundId= @MFID

	INSERT INTO #Temp_Id
	SELECT MFPrefVal.MasterFundId FROM T_AL_MFAllocationPreference AS MFPref JOIN T_AL_MasterFundProrataList AS MFPrefVal
	ON MFPref.MFPreferenceId = MFPrefVal.MFPreferenceId AND MFPref.CompanyId= @companyID AND MFPrefVal.MFPreferenceId= @MFID

	INSERT INTO #Temp_Id
	SELECT MasterFundID FROM T_AllocationMasterfundRatio
	WHERE MasterFundID= @MFID AND TargetRatioPct > 0

	SELECT DISTINCT MasterFundId FROM #Temp_Id

	DROP TABLE #Temp_Id

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