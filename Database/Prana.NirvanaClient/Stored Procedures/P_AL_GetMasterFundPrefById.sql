CREATE PROCEDURE [dbo].[P_AL_GetMasterFundPrefById]
(
	@MasterFundPreferenceId int
)
AS

SELECT AP.MFPreferenceId
		,AP.MFPreferenceName
		,AP.CompanyId
		,AP.UpdateDateTime
		,AP.AllocationBase			
		,AP.MatchingRule			
		,AP.MatchPortfolioPosition	
		,AP.PreferencedFundId		
		,AP.ProrataDaysBack 
		,(
			select MasterFundId as FundId
			from T_AL_MasterFundProrataList as TPFL
			where TPFL.MFPreferenceId = AP.MFPreferenceId
			FOR XML AUTO, ELEMENTS, ROOT('ProrataFundList')
		) as ProrataFundList
		,(
			Select Distinct (Select MFId, CalculatedPrefId 
			FROM T_AL_MFWisePrefValues 
			WHERE MFPreferenceId = AP.MFPreferenceId FOR XML AUTO, ELEMENTS, ROOT('MasterFundCalculatedPref'))
		) AS MasterFundCalculatedPref
		,(
			Select Distinct (Select MFId, Value 
			FROM T_AL_MFWisePrefValues 
			WHERE MFPreferenceId = AP.MFPreferenceId FOR XML AUTO, ELEMENTS, ROOT('MasterFundTargetPercentage'))
		) AS MasterFundTargetPercentage
FROM T_AL_MFAllocationPreference AS AP
WHERE AP.MFPreferenceId = @MasterFundPreferenceId

BEGIN TRY
BEGIN TRAN

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
	
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH
