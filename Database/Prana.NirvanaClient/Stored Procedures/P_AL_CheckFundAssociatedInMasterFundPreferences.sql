CREATE PROCEDURE [dbo].[P_AL_CheckFundAssociatedInMasterFundPreferences]
(
@companyId int,
@MFID int,
@accountId int
)
AS

BEGIN TRY
	
BEGIN TRAN

	CREATE TABLE #Temp_MFId
	(
		MFPrefId int
	)

	INSERT INTO #Temp_MFId
	SELECT Id FROM T_AL_AllocationPreferenceDef 
		WHERE Id IN (SELECT CalculatedPrefId FROM T_AL_MFAllocationPreference AS MFPref JOIN T_AL_MFWisePrefValues AS MFPrefVal
										ON MFPref.MFPreferenceId = MFPrefVal.MFPreferenceId AND MFPref.CompanyId= @companyId AND MFPrefVal.MFId= @MFID) 
	

	CREATE TABLE #Temp_Funds
	(
		FundsId int
	)

	INSERT INTO #Temp_Funds
	SELECT FundId FROM T_AL_AllocationPreferenceData 
	WHERE PresetdefId IN (SELECT MFPrefId FROM #Temp_MFId) AND FundId = @accountId

	INSERT INTO #Temp_Funds
	SELECT FundId FROM T_AL_ProrataFundList
	WHERE PreferenceId IN (SELECT MFPrefId FROM #Temp_MFId) AND FundId = @accountId

	INSERT INTO #Temp_Funds
	SELECT AccountId FROM T_AL_AccountCheckListValue
	WHERE CheckListId IN (SELECT CheckListId FROM T_AL_CheckList WHERE PresetDefId IN (SELECT MFPrefId FROM #Temp_MFId) )
			AND AccountId = @accountId

	INSERT INTO #Temp_Funds
	SELECT FundId FROM T_AL_ProrataFundList
	WHERE CheckListId IN (SELECT CheckListId FROM T_AL_CheckList WHERE PresetDefId IN (SELECT MFPrefId FROM #Temp_MFId) )
			AND FundId = @accountId

	INSERT INTO #Temp_Funds
	SELECT PreferencedFundId FROM T_AL_CheckList
	WHERE PresetDefId IN (SELECT MFPrefId FROM #Temp_MFId)
			AND PreferencedFundId = @accountId

	INSERT INTO #Temp_Funds
	SELECT PreferencedFundId FROM T_AL_AllocationPreferenceDef 
		WHERE Id IN (SELECT CalculatedPrefId FROM T_AL_MFAllocationPreference AS MFPref JOIN T_AL_MFWisePrefValues AS MFPrefVal
										ON MFPref.MFPreferenceId = MFPrefVal.MFPreferenceId AND MFPref.CompanyId= @companyId AND MFPrefVal.MFId= @MFID)
							AND PreferencedFundId = @accountId

	SELECT DISTINCT FundsId FROM #Temp_Funds

COMMIT

	DROP TABLE #Temp_Funds
	DROP TABLE #Temp_MFId

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