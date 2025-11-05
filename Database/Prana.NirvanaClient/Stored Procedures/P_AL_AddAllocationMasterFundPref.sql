CREATE PROCEDURE [dbo].[P_AL_AddAllocationMasterFundPref]
(
	@CompanyId	INT,
	@NAME		NVARCHAR(200)	
)
AS
BEGIN TRY
BEGIN TRAN

DECLARE @NewPresetDefId INT

INSERT INTO T_AL_MFAllocationPreference
	(
		[MFPreferenceName]
		,[CompanyId]
		,[UpdateDateTime]
		,[AllocationBase]			
		,[MatchingRule]				
		,[MatchPortfolioPosition]	
		,[PreferencedFundId]		
		,[ProrataDaysBack]
	)
VALUES(	@NAME
		,@CompanyId
		,GETDATE()
		,1				
		,1					
		,0		
		,NULL				
		,0
	  ) 

SELECT @NewPresetDefId = SCOPE_IDENTITY()

EXEC P_AL_GetMasterFundPrefById @NewPresetDefId

COMMIT
END TRY

BEGIN CATCH
	ROLLBACK
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

     RAISERROR (
				@ErrorMessage,
				@ErrorSeverity,
				@ErrorState
               );
END CATCH