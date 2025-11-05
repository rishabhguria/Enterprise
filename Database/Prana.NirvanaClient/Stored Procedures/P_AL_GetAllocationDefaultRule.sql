
CREATE PROCEDURE [dbo].[P_AL_GetAllocationDefaultRule]

AS

BEGIN TRY
BEGIN TRAN

SELECT 
	 def.[CompanyId],def.[AllocationBase]
	,def.[MatchingRule],def.[MatchPortfolioPosition]
	,def.[PreferencedFundId]
	,def.[DoCheckSide]
	,def.[AllowEditPreferences]
	,def.[ProrataFundList]
	,def.[ProrataDaysBack]
	,def.[CheckSidePreference]
	,def.[PrecisionDigit]
	,def.[AssetsWithCommissionInNetAmount]
	,def.[MsgOnBrokerChange]			
	,def.[RecalculateOnBrokerChange]	
	,def.[MsgOnAllocation]			
	,def.[RecalculateOnAllocation]
	,def.[EnableMasterFundAllocation]
	,def.[IsOneSymbolOneMasterFundAllocation]	
	,def.[ProrataSchemeName]
	,def.[AllocationSchemeKey]
	,def.[SetSchemeFromUI]	
FROM T_AL_AllocationDefaultRule AS def

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

