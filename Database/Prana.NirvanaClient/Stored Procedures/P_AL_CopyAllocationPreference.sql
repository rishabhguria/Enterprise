
CREATE PROCEDURE [dbo].[P_AL_CopyAllocationPreference]
(
	@PreferenceIdToCopyFrom int,
	@NewNAME nvarchar(100)	
)
AS

BEGIN TRY

DECLARE @NewPresetDefId int
set @NewPresetDefId = -1
DECLARE @intErrorCode INT
DECLARE @PositionPriority INT

BEGIN TRAN

--DECLARE @NewPresetDefId int
--Declare @NewPresetDefId int


------------------------------------------------------------------------------------
--------------- Inserting into T_AL_AllocationPreferenceDef table ------------------
------------------------------------------------------------------------------------
SET @PositionPriority = ISNULL((SELECT MAX(PositionPriority) FROM T_AL_AllocationPreferenceDef),0) + 1

insert into T_AL_AllocationPreferenceDef
	(
		[Name],[CompanyId],[AllocationBase],[MatchingRule],[MatchPortfolioPosition],
		[PreferencedFundId],[UpdateDateTime], [ProrataDaysBack], [PositionPriority], [IsPrefVisible]
	)
SELECT	
		@NewNAME, def.CompanyId, def.AllocationBase,def.MatchingRule,
		def.MatchPortfolioPosition,def.PreferencedFundId, GETDATE(), def.ProrataDaysBack, @PositionPriority, def.IsPrefVisible
from T_AL_AllocationPreferenceDef as def 
where def.Id= @PreferenceIdToCopyFrom
SELECT @NewPresetDefId =SCOPE_IDENTITY()

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------


--print(@NewPresetDefId)

------------------------------------------------------------------------------------
------------- Copying into temp AllocationPreferenceData table ---------------------
------------------------------------------------------------------------------------

Select *
Into   #TempAllocationPreferenceData
From   T_AL_AllocationPreferenceData as data
where data.PresetDefId= @PreferenceIdToCopyFrom

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
------------- Inserting into AllocationPreferenceData table ------------------------
------------- as well as strategy table --------------------------------------------
------------------------------------------------------------------------------------

DECLARE @NewPrefDataId int
DECLARE @CurrentPrefDataId int

While (Select Count(*) From #TempAllocationPreferenceData) > 0
Begin

	Select Top 1 @CurrentPrefDataId = Id From #TempAllocationPreferenceData

    insert into T_AL_AllocationPreferenceData([PresetDefId],[FundId],[Value])
	SELECT @NewPresetDefId,[FundId],[Value]
	from T_AL_AllocationPreferenceData as data
	where data.Id= @CurrentPrefDataId
	SELECT @NewPrefDataId =SCOPE_IDENTITY()

	INSERT INTO T_AL_StrategyValue ([AllocationPrefDataId],[StrategyId],[Value])
	SELECT @NewPrefDataId, [StrategyId],[Value]
	FROM T_AL_StrategyValue
	WHERE AllocationPrefDataId = @CurrentPrefDataId 


	DELETE from #TempAllocationPreferenceData WHERE Id = @CurrentPrefDataId

End

INSERT INTO T_AL_ProrataFundList ([CheckListId],[PreferenceId],[FundId])
	SELECT -1,@NewPresetDefId, [FundId] 
	FROM T_AL_ProrataFundList
	WHERE CheckListId = -1 and PreferenceId = @PreferenceIdToCopyFrom 


------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
------------- Copying into temp AllocationCheckList table --------------------------
------------------------------------------------------------------------------------

Select *
Into   #TempAllocationCheckList
From   T_AL_CheckList as data
where data.PresetDefId= @PreferenceIdToCopyFrom

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
CREATE TABLE #TempT_AL_AccountCheckListValue
(
	[Id] INT NOT NULL,
	[CheckListId] INT NOT NULL,
	[AccountId] INT NOT NULL,	
	[Value] decimal(32,19) NOT NULL
)

-------------------------------------------------------------------------------
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
---- Inserting into temp #TempT_AL_AccountCheckListValue table from XML ----
-------------------------------------------------------------------------------

insert into #TempT_AL_AccountCheckListValue
	(
		[Id],[CheckListId], [AccountId], [Value]
	)
	SELECT Id,CheckListId, AccountId, Value
	FROM T_AL_AccountCheckListValue
	WHERE CheckListId IN (SELECT CheckListId FROM #TempAllocationCheckList)

-------------------------------------------------------------------------------
-------------------------------------------------------------------------------

------------------------------------------------------------------------------------
------------- Inserting into AllocationCheckList table -----------------------------
------------- as well as asset, exchnage and PR table ------------------------------
------------------------------------------------------------------------------------

Declare @Id int
DECLARE @NewCheckListId int

While (Select Count(*) From #TempAllocationCheckList) > 0
Begin

	Select Top 1 @Id = CheckListId From #TempAllocationCheckList

    insert into T_AL_CheckList
	(
		[PresetDefId],[ExchangeOperator],[AssetOperator],[PROperator],[AllocationBase],
		[MatchingRule],[MatchPortfolioPosition], [PreferencedFundId], [ProrataDaysBack],[OrderSideOperator]
	)
	SELECT @NewPresetDefId,[ExchangeOperator],[AssetOperator],[PROperator],[AllocationBase],
			[MatchingRule],[MatchPortfolioPosition], [PreferencedFundId], [ProrataDaysBack],[OrderSideOperator]
	from T_AL_CheckList as data
	where data.CheckListId= @Id
	SELECT @NewCheckListId =SCOPE_IDENTITY()

	INSERT INTO T_AL_Asset ([CheckListId],[AssetId])
	SELECT @NewCheckListId, [AssetId] 
	FROM T_AL_Asset
	WHERE CheckListId = @Id 

	INSERT INTO T_AL_Exchange ([CheckListId],[ExchangeId])
	SELECT @NewCheckListId, [ExchangeId] 
	FROM T_AL_Exchange
	WHERE CheckListId = @Id 

	INSERT INTO T_AL_PR ([CheckListId],[PR])
	SELECT @NewCheckListId, [PR] 
	FROM T_AL_PR
	WHERE CheckListId = @Id 

	INSERT INTO T_AL_ProrataFundList ([CheckListId],[PreferenceId],[FundId])
	SELECT @NewCheckListId,@NewPresetDefId, [FundId] 
	FROM T_AL_ProrataFundList
	WHERE CheckListId = @Id-- && PreferenceId = @PreferenceIdToCopyFrom 

	INSERT INTO T_AL_OrderSide([CheckListId],[OrderSideId])
	SELECT @NewCheckListId, [OrderSideId] 
	FROM T_AL_OrderSide
	WHERE CheckListId = @Id 

	DECLARE @NewAccountCheckListValueId int
	DECLARE @CurrentAccountId int
	While (Select Count(*) From #TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id) > 0
	Begin

		Select Top 1 @CurrentAccountId = Id From #TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id
	
		insert into T_AL_AccountCheckListValue
		(
			[CheckListId],[AccountId],[Value]
		)
		SELECT 
				@NewChecklistId,[AccountId],[Value]
		FROM  #TempT_AL_AccountCheckListValue WHERE Id = @CurrentAccountId AND ChecklistId = @Id
		SELECT @NewAccountCheckListValueId =SCOPE_IDENTITY()

		insert into T_AL_StrategyChecklistValues([AccountCheckListId],[StrategyId],[Value])
		SELECT @NewAccountCheckListValueId,[StrategyId],[Value]
		FROM  T_AL_StrategyChecklistValues WHERE AccountCheckListId = @CurrentAccountId 
		
		DELETE from #TempT_AL_AccountCheckListValue WHERE Id = @CurrentAccountId AND CheckListId=@Id
	
	End

	DELETE from #TempAllocationCheckList WHERE CheckListId = @Id

End

DROP TABLE #TempT_AL_AccountCheckListValue
DROP TABLE #TempAllocationPreferenceData
DROP TABLE #TempAllocationCheckList

COMMIT
-- Problem section dropping if temporary table has been created

Exec P_AL_GetAllocationPreferenceById @NewPresetDefId
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

