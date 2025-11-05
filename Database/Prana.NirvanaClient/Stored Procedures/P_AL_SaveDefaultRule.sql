
CREATE PROCEDURE [dbo].[P_AL_SaveDefaultRule] 
	@CompanyId							INT,	
	@AllocationBase						INT,
	@MatchingRule						INT,
	@MatchPortfolioPosition				INT,
	@PreferencedFundId					INT,
	@AllowEditPreferences				BIT,
	@ProrataFundList					NVARCHAR(MAX),
	@ProrataDaysBack					INT,
	@PrecisionDigit						INT,
	@AssetsWithCommissionInNetAmount	NVARCHAR(MAX),
	@MsgOnBrokerChange					BIT,	
	@RecalculateOnBrokerChange			BIT,
	@MsgOnAllocation					BIT,		
	@RecalculateOnAllocation			BIT,
	@EnableMasterFundAllocation         BIT,
	@IsOneSymbolOneMasterFundAllocation BIT,
	@ProrataSchemeName					NVARCHAR(MAX),
	@AllocationSchemeKey				INT,
	@SetSchemeFromUI					BIT,
	@CheckSidePreference			    NVARCHAR(MAX)
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN
	IF NOT EXISTS (SELECT * FROM dbo.T_AL_AllocationDefaultRule WHERE CompanyId = @CompanyId)
       INSERT INTO T_AL_AllocationDefaultRule
			(
				CompanyId,	
				AllocationBase,
				MatchingRule,
				MatchPortfolioPosition,
				PreferencedFundId,
				AllowEditPreferences,
				ProrataFundList,
				ProrataDaysBack,
				PrecisionDigit,
				AssetsWithCommissionInNetAmount,
				MsgOnBrokerChange,	
				RecalculateOnBrokerChange,
				MsgOnAllocation,		
				RecalculateOnAllocation,
				EnableMasterFundAllocation, 
				IsOneSymbolOneMasterFundAllocation,
				ProrataSchemeName,
				AllocationSchemeKey,
				SetSchemeFromUI,
				CheckSidePreference
            )
     VALUES
           (
				@CompanyId,	
				@AllocationBase,
				@MatchingRule,
				@MatchPortfolioPosition,
				CASE 
					WHEN @PreferencedFundId=-1 THEN NULL
					ELSE @PreferencedFundId 
				END  ,
				@AllowEditPreferences,
				@ProrataFundList,
				@ProrataDaysBack,
				@PrecisionDigit,
				@AssetsWithCommissionInNetAmount,
				@MsgOnBrokerChange,	
				@RecalculateOnBrokerChange,
				@MsgOnAllocation,		
				@RecalculateOnAllocation,	
				@EnableMasterFundAllocation,
	            @IsOneSymbolOneMasterFundAllocation,
				@ProrataSchemeName					,
				@AllocationSchemeKey			,
				@SetSchemeFromUI,
				@CheckSidePreference
            )
    ELSE
		UPDATE T_AL_AllocationDefaultRule
		SET
			AllocationBase = @AllocationBase,
			MatchingRule = @MatchingRule,
			MatchPortfolioPosition = @MatchPortfolioPosition,
			PreferencedFundId = 
				CASE 
					WHEN @PreferencedFundId=-1 THEN NULL
					ELSE @PreferencedFundId 
				END,
			
			AllowEditPreferences=@AllowEditPreferences,
			ProrataFundList=@ProrataFundList,
			ProrataDaysBack=@ProrataDaysBack,
			PrecisionDigit=@PrecisionDigit,
			AssetsWithCommissionInNetAmount=@AssetsWithCommissionInNetAmount,
			MsgOnBrokerChange = @MsgOnBrokerChange,	
			RecalculateOnBrokerChange = @RecalculateOnBrokerChange,
			MsgOnAllocation = @MsgOnAllocation,		
			RecalculateOnAllocation	 = @RecalculateOnAllocation,
			EnableMasterFundAllocation= @EnableMasterFundAllocation,        
	        IsOneSymbolOneMasterFundAllocation= @IsOneSymbolOneMasterFundAllocation ,
			ProrataSchemeName=@ProrataSchemeName,
			AllocationSchemeKey=@AllocationSchemeKey,
			SetSchemeFromUI=@SetSchemeFromUI,
			CheckSidePreference=@CheckSidePreference
           WHERE CompanyId = @CompanyId

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
	

    -- Use RAISERROR inside the CATCH block to return 
    -- error information about the original error that 
    -- caused execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH

