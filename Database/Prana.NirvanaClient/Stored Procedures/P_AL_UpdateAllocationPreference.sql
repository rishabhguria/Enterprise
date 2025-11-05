CREATE Procedure [P_AL_UpdateAllocationPreference]
(
	@PrefXML nText	
)
AS

DECLARE @intErrorCode INT,@NewPresetDefId int,@Id int,@NewChecklistId int,@handle int

exec sp_xml_preparedocument @handle OUTPUT,@PrefXML 

----------------- Creating @TempAllocationPreferenceDef tables ----------------

Declare @TempAllocationPreferenceDef TABLE
(
	[Id] INT NOT NULL, 
	[Name] nvarchar(200) NOT NULL,
	[CompanyId] INT NOT NULL,	
	[AllocationBase] INT NOT NULL,
	[MatchingRule] INT NOT NULL,
	[MatchPortfolioPosition] INT NOT NULL,
	[PreferencedFundId] INT NULL,
	[UpdateDateTime] datetime NOT NULL,
	--[ProrataFundList] xml NULL,
	[ProrataDaysBack] int NULL,
	[IsPrefVisible] BIT NOT NULL
)

-------- Creating @TempT_AL_AllocationPreferenceData table for checkLists -----

Declare @TempT_AL_AllocationPreferenceData TABLE
(	
	[PresetDefId] INT NOT NULL,
	[FundId] INT NOT NULL,	
	[Value] decimal(32,19) NOT NULL
)

------------- Creating @TempT_AL_StrategyValue table for checkLists -------------------

Declare @TempT_AL_StrategyValue TABLE
(
	[AllocationPrefDataId] int NOT NULL,
	[StrategyId] INT NULL, 
	[Value] decimal(32,19) NULL	
)

------------- Creating @TempT_AL_CheckList table for checkLists ---------------

Declare @TempT_AL_CheckList TABLE
(
	[ChecklistId] INT NOT NULL, 
	[PresetDefId] INT NOT NULL,
	[ExchangeOperator] INT NOT NULL,	
	[AssetOperator] INT NOT NULL,
	[PROperator] INT NOT NULL,	
	[AllocationBase] INT NOT NULL,
	[MatchingRule] INT NOT NULL,
	[MatchPortfolioPosition] INT NOT NULL,
	[PreferencedFundId] INT NULL,
	--[ProrataFundList] xml NULL,
	[ProrataDaysBack] INT NULL,
	[OrderSideOperator] INT NOT NULL
)

------------- Creating @TempT_AL_Asset table for checkLists -------------------

Declare @TempT_AL_Asset TABLE
(
	[ChecklistId] INT NOT NULL, 
	[AssetId] INT NULL	
)

------------- Creating @TempT_AL_Exchange table for checkLists ----------------

Declare @TempT_AL_Exchange TABLE
(
	[ChecklistId] INT NOT NULL, 
	[ExchangeId] INT NULL	
)

------------- Creating @TempT_AL_PR table for checkLists ----------------------

Declare @TempT_AL_PR TABLE
(
	[ChecklistId] INT NOT NULL, 
	[PR] nvarchar(50) NULL	
)

------------- Creating @TempT_AL_ProrataFundList table for checkLists -------------------

Declare @TempT_AL_ProrataFundList TABLE
(
	[ChecklistId] INT NOT NULL, 
	[FundId] INT NULL	
)

------------- Creating @TempT_AL_OrderSide table for checkLists ----------------

Declare @TempT_AL_OrderSide TABLE
(
	[ChecklistId] INT NOT NULL, 
	[OrderSideId] nvarchar(50) NULL	
)

-------- Creating @TempT_AL_AccountCheckListValue table for checkLists -----

Declare @TempT_AL_AccountCheckListValue TABLE
(	
	[CheckListId] INT NOT NULL,
	[AccountId] INT NOT NULL,	
	[Value] decimal(32,19) NOT NULL
)

------------- Creating @TempT_AL_StrategyChecklistValues table for checkLists -------------------

Declare @TempT_AL_StrategyChecklistValues TABLE
(
	[AccountCheckListId] INT NOT NULL,
	[StrategyId] INT NULL, 
	[Value] decimal(32,19) NULL	,
	[CheckListId] INT NOT NULL
)

--------------- Inserting into @TempAllocationPreferenceDef table -------------

insert into @TempAllocationPreferenceDef
	(
		[Id], [Name],[CompanyId],
		[AllocationBase],[MatchingRule],[MatchPortfolioPosition], [PreferencedFundId],
		[UpdateDateTime],[ProrataDaysBack], [IsPrefVisible]
	)
SELECT 
		OperationPreferenceId, OperationPreferenceName, CompanyId, 
		(
			select id 
			from T_AL_AllocationBase as B with (NOLOCK)
			where B.AllocationBase =xm.AllocationBase
		) as AllocationBase,		
		(
			select id 
			from T_AL_MatchingRule as B with (NOLOCK)
			where B.MatchingRule =xm.MatchingRule
		) as MatchingRule,
		(
			SELECT id 
			FROM T_AL_MatchPortfolioPosition AS B with (NOLOCK)
			WHERE B.MatchPortfolioPosition=xm.MatchPortfolioPosition
		) AS MatchPortfolioPosition, 
		case 
			when xm.PreferencedFundId=-1 then NULL
			else xm.PreferencedFundId 
		end as PreferencedFundId,
		GETDATE(),-- update time will be current date time
		--xm.ProrataFundList as ProrataFundList,
		xm.ProrataDaysBack as ProrataDaysBack,
		xm.IsPrefVisible as IsPrefVisible
FROM  OPENXML(@handle, '/AllocationOperationPreference',2)                                                                                                                                  
WITH
(
	OperationPreferenceId int,
	OperationPreferenceName NVARCHAR(200),
	CompanyId int,
	AllocationBase nvarchar(50) '/AllocationOperationPreference/DefaultRule/BaseType',
	MatchingRule nvarchar(50) '/AllocationOperationPreference/DefaultRule/RuleType',
	MatchPortfolioPosition nvarchar(50) '/AllocationOperationPreference/DefaultRule/MatchClosingTransaction',
	PreferencedFundId nvarchar(50) '/AllocationOperationPreference/DefaultRule/PreferenceAccountId',
	--ProrataFundList nvarchar(max) '/AllocationOperationPreference/DefaultRule/ProrataAccountList',
	ProrataDaysBack int '/AllocationOperationPreference/DefaultRule/ProrataDaysBack',
	IsPrefVisible BIT 
) as xm

select TOP 1 @NewPresetDefId = Id  from @TempAllocationPreferenceDef

---- Inserting into temp @TempT_AL_AllocationPreferenceData table from XML ----

insert into @TempT_AL_AllocationPreferenceData
	(
		[PresetdefId], [FundId], [Value]
	)
SELECT 
		@NewPresetDefId, [FundId], [Value]
FROM  OPENXML(@handle, '/AllocationOperationPreference/TargetPercentage/KeyValuePair',2)                                                                                                                                  
WITH
(	
	[FundId] int 'Value/AccountId',
	[Value] decimal(32,19)'Value/Value'
) as xm where [Value]>0

---------- Inserting into temp @TempT_AL_Asset table from XML -----------------

insert into @TempT_AL_StrategyValue	( [AllocationPrefDataId], [StrategyId],[Value] )
SELECT [AllocationPrefDataId], [StrategyId],[Value]
FROM  OPENXML(@handle, '/AllocationOperationPreference/TargetPercentage/KeyValuePair/Value/StrategyValueList/StrategyValue',2)
WITH
(	
	[AllocationPrefDataId] int '../../AccountId',
	[StrategyId] int 'StrategyId',
	[Value] decimal(32,19) 'Value'	
) as xm where [Value]>0

---------- Inserting into temp @TempT_AL_CheckList table from XML -------------

insert into @TempT_AL_CheckList
	( 
		[ChecklistId],[PresetDefId], [ExchangeOperator], [AssetOperator], [PROperator],
		[AllocationBase], [MatchingRule], [MatchPortfolioPosition],	[PreferencedFundId],
		[ProrataDaysBack],[OrderSideOperator]
	)
SELECT 
		[ChecklistId],@NewPresetDefId, 
		(
			select id 
			from T_AL_Operator as B with (NOLOCK)
			where B.Operator = xm.ExchangeOperator
		) as [ExchangeOperator], 
		(
			select id 
			from T_AL_Operator as B with (NOLOCK)
			where B.Operator = xm.AssetOperator
		) as [AssetOperator], 
		(
			select id 
			from T_AL_Operator as B with (NOLOCK)
			where B.Operator = xm.PROperator
		) as [PROperator],
		(
			select id 
			from T_AL_AllocationBase as B with (NOLOCK)
			where B.AllocationBase =xm.AllocationBase
		) as AllocationBase,
		(
			select id 
			from T_AL_MatchingRule as B with (NOLOCK)
			where B.MatchingRule =xm.MatchingRule
		) as MatchingRule,
		(
			SELECT id 
			FROM T_AL_MatchPortfolioPosition AS B with (NOLOCK)
			WHERE B.MatchPortfolioPosition=xm.MatchPortfolioPosition
		) AS MatchPortfolioPosition, 
		case 
			when xm.PreferencedFundId=-1 then NULL
			else xm.PreferencedFundId 
		end as PreferencedFundId,
		--[ProrataFundList],
		[ProrataDaysBack],
		(
			select id 
			from T_AL_Operator as B with (NOLOCK)
			where B.Operator = xm.OrderSideOperator
		) as [OrderSideOperator]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair',2)
WITH
(	
	[ChecklistId] int 'Value/ChecklistId',
	[ExchangeOperator] nvarchar(50) 'Value/ExchangeOperator',
	[AssetOperator] nvarchar(50) 'Value/AssetOperator',
	[PROperator] nvarchar(50) 'Value/PROperator',
	[AllocationBase] nvarchar(50) 'Value/Rule/BaseType',
	[MatchingRule] nvarchar(50) 'Value/Rule/RuleType',
	[MatchPortfolioPosition] nvarchar(50) 'Value/Rule/MatchClosingTransaction',
	[PreferencedFundId] int 'Value/Rule/PreferenceAccountId',
	--[ProrataFundList] xml 'Value/Rule/ProrataAccountList',
	[ProrataDaysBack] int 'Value/Rule/ProrataDaysBack',
	[OrderSideOperator] nvarchar(50) 'Value/OrderSideOperator'
) as xm

---------- Inserting into temp @TempT_AL_Asset table from XML -----------------

insert into @TempT_AL_Asset	( [ChecklistId],[AssetId] )
SELECT [ChecklistId],[AssetId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/AssetList/Int32',2)
WITH
(	
	[ChecklistId] int '../../ChecklistId',
	[AssetId] int '.'	
) as xm

---------- Inserting into temp @TempT_AL_Exchange table from XML --------------

insert into @TempT_AL_Exchange	( [ChecklistId],[ExchangeId] )
SELECT [ChecklistId],[ExchangeId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/ExchangeList/Int32',2)
WITH
(	
	[ChecklistId] int '../../ChecklistId',
	[ExchangeId] int '.'	
) as xm

---------- Inserting into temp @TempT_AL_PR table from XML -----------------

insert into @TempT_AL_PR	( [ChecklistId],[PR] )
SELECT [ChecklistId],[PR]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/PRList/String',2)
WITH
(	
	[ChecklistId] int '../../ChecklistId',
	[PR] NVARCHAR(50) '.'	
) as xm

---------- Inserting into temp @TempT_AL_ProrataFundList table from XML -----------------

insert into @TempT_AL_ProrataFundList	( [ChecklistId],[FundId] )
SELECT [ChecklistId],[FundId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/Rule/ProrataAccountList/Int32',2)
WITH
(	
	[ChecklistId] int '../../../ChecklistId',
	[FundId] int '.'	
) as xm

---------- Inserting into temp @TempT_AL_OrderSide table from XML --------------

insert into @TempT_AL_OrderSide	( [ChecklistId],[OrderSideId] )
SELECT [ChecklistId],[OrderSideId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/OrderSideList/String',2)
WITH
(	
	[ChecklistId] int '../../ChecklistId',
	[OrderSideId] NVARCHAR(50) '.'	
) as xm

---- Inserting into temp @TempT_AL_AccountCheckListValue table from XML ----

insert into @TempT_AL_AccountCheckListValue
	(
		[CheckListId], [AccountId], [Value]
	)
SELECT 
		[ChecklistId], [AccountId], [Value]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/TargetPercentage/KeyValuePair',2)                                                                                                                            
WITH
(	
	[ChecklistId] int '../../ChecklistId',
	[AccountId] int 'Value/AccountId',
	[Value] decimal(32,19)'Value/Value'
) as xm where [Value]>0

---------- Inserting into temp @TempT_AL_StrategyChecklistValues table from XML -----------------

insert into @TempT_AL_StrategyChecklistValues ( [AccountCheckListId], [StrategyId],[Value],[ChecklistId] )
SELECT [AccountCheckListId], [StrategyId],[Value],[ChecklistId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/TargetPercentage/KeyValuePair/Value/StrategyValueList/StrategyValue',2)
WITH
(	
	[AccountCheckListId] int '../../AccountId',
	[StrategyId] int 'StrategyId',
	[Value] decimal(32,19) 'Value'	,
	[ChecklistId] int '../../../../../ChecklistId'
) as xm where [Value]>0


BEGIN TRY

BEGIN TRAN

-- Updating T_AL_AllocationPreferenceDef table from temp table ----------------

Update T_AL_AllocationPreferenceDef
SET 
	[Name] = tempTab.[Name],
	[CompanyId] = tempTab.[CompanyId],
	[AllocationBase] = tempTab.[AllocationBase],
	[MatchingRule] = tempTab.[MatchingRule],
	[MatchPortfolioPosition] = tempTab.[MatchPortfolioPosition], 
	[PreferencedFundId] = tempTab.[PreferencedFundId],
	[UpdateDateTime] = tempTab.[UpdateDateTime],
	--[ProrataFundList]= tempTab.[ProrataFundList],
	[ProrataDaysBack] = tempTab.[ProrataDaysBack],
	[IsPrefVisible] = tempTab.[IsPrefVisible]
FROM @TempAllocationPreferenceDef as tempTab
WHERE T_AL_AllocationPreferenceDef.Id = tempTab.Id

---------- Inserting into temp #T_AL_ProrataFundList table from XML -----------------

delete from T_AL_ProrataFundList where [ChecklistId]=-1 and [PreferenceId] = @NewPresetDefId

insert into T_AL_ProrataFundList ( [ChecklistId],[PreferenceId],[FundId] )
SELECT -1,@NewPresetDefId,[FundId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/DefaultRule/ProrataAccountList/Int32',2)
WITH
(	
	[FundId] int '.'	
) as xm

---- Deleting data tables from T_AL_AllocationPreferenceData table from XML ---

Delete from T_AL_StrategyValue
Where AllocationPrefDataId in (select Id from T_AL_AllocationPreferenceData  where PresetDefId=@NewPresetDefId)
DELETE from T_AL_AllocationPreferenceData where PresetDefId=@NewPresetDefId

-------- Updating tables from temp @TempT_AL_CheckList table from XML ---------

DECLARE @NewPrefDataId int
DECLARE @CurrentFundId int

While (Select Count(*) From @TempT_AL_AllocationPreferenceData) > 0
Begin

	Select Top 1 @CurrentFundId = FundId From @TempT_AL_AllocationPreferenceData
	
	insert into T_AL_AllocationPreferenceData
	(
		[PresetDefId],[FundId],[Value]
	)
	SELECT 
			@NewPresetDefId,[FundId],[Value]
	FROM  @TempT_AL_AllocationPreferenceData WHERE FundId = @CurrentFundId
	SELECT @NewPrefDataId =SCOPE_IDENTITY()

	insert into T_AL_StrategyValue([AllocationPrefDataId],[StrategyId],[Value])
	SELECT @NewPrefDataId,[StrategyId],[Value]
	FROM  @TempT_AL_StrategyValue WHERE AllocationPrefDataId = @CurrentFundId 

	DELETE from @TempT_AL_AllocationPreferenceData WHERE FundId = @CurrentFundId
	
End

-------- Deleting data tables from temp T_AL_CheckList table from XML ----

DECLARE @results1 TABLE (a INT not null)

INSERT INTO @results1 Select ChecklistId from T_AL_CheckList where PresetDefId=@NewPresetDefId
Delete from T_AL_Asset where CheckListId IN (Select a from @results1)
DELETE from T_AL_Exchange where CheckListId IN (Select a from @results1)
DELETE from T_AL_PR where CheckListId IN (Select a from @results1)
DELETE from T_AL_ProrataFundList where CheckListId IN (Select a from @results1)
DELETE from T_AL_OrderSide where CheckListId IN (Select a from @results1)
DELETE from T_AL_StrategyChecklistValues where AccountCheckListId IN (Select Id from T_AL_AccountCheckListValue where CheckListId IN (Select a from @results1))
DELETE from T_AL_AccountCheckListValue WHERE CheckListId IN (Select a from @results1)
DELETE from T_AL_CheckList WHERE CheckListId IN (Select a from @results1)

-------- Updating tables from temp @TempT_AL_CheckList table from XML ---------

While (Select Count(*) From @TempT_AL_CheckList) > 0
Begin

	Select Top 1 @Id = ChecklistId From @TempT_AL_CheckList
	
	insert into T_AL_CheckList
	(
		[PresetDefId],[ExchangeOperator],[AssetOperator],[PROperator],[AllocationBase],
		[MatchingRule],[MatchPortfolioPosition], [PreferencedFundId],[ProrataDaysBack],[OrderSideOperator]
	)
	SELECT 
			@NewPresetDefId,[ExchangeOperator],[AssetOperator],[PROperator],[AllocationBase],
			[MatchingRule],[MatchPortfolioPosition], [PreferencedFundId],[ProrataDaysBack],[OrderSideOperator]
	FROM  @TempT_AL_CheckList WHERE ChecklistId = @Id
	SELECT @NewChecklistId =SCOPE_IDENTITY()

	insert into T_AL_Asset([ChecklistId],[AssetId])
	SELECT @NewChecklistId,[AssetId]
	FROM  @TempT_AL_Asset WHERE ChecklistId = @Id and AssetId is not NULL

	insert into T_AL_Exchange([ChecklistId],[ExchangeId])
	SELECT @NewChecklistId,[ExchangeId]
	FROM  @TempT_AL_Exchange WHERE ChecklistId = @Id and ExchangeId IS NOT NULL

	insert into T_AL_PR([ChecklistId],[PR])
	SELECT @NewChecklistId,[PR]
	FROM  @TempT_AL_PR WHERE ChecklistId = @Id and PR IS NOT NULL

	insert into T_AL_ProrataFundList([ChecklistId],[PreferenceId],[FundId])
	SELECT @NewChecklistId,@NewPresetDefId,[FundId]
	FROM  @TempT_AL_ProrataFundList WHERE ChecklistId = @Id --and FundId IS NOT NULL

	insert into T_AL_OrderSide([ChecklistId],[OrderSideId])
	SELECT @NewChecklistId,[OrderSideId]
	FROM  @TempT_AL_OrderSide WHERE ChecklistId = @Id and OrderSideId IS NOT NULL
	-------- Updating tables from temp @TempT_AL_AccountCheckListValue table from XML
	DECLARE @NewAccountCheckListValueId int
	DECLARE @CurrentAccountId int
	While (Select Count(*) From @TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id) > 0
	Begin

		Select Top 1 @CurrentAccountId = AccountId From @TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id
	
		insert into T_AL_AccountCheckListValue
		(
			[CheckListId],[AccountId],[Value]
		)
		SELECT 
				@NewChecklistId,[AccountId],[Value]
		FROM  @TempT_AL_AccountCheckListValue WHERE AccountId = @CurrentAccountId AND ChecklistId = @Id
		SELECT @NewAccountCheckListValueId =SCOPE_IDENTITY()
		
		insert into T_AL_StrategyChecklistValues([AccountCheckListId],[StrategyId],[Value])
		SELECT @NewAccountCheckListValueId,[StrategyId],[Value]
		FROM  @TempT_AL_StrategyChecklistValues WHERE AccountCheckListId = @CurrentAccountId and CheckListId=@Id
		
		DELETE from @TempT_AL_StrategyChecklistValues WHERE  AccountCheckListId = @CurrentAccountId and CheckListId=@Id
		DELETE from @TempT_AL_AccountCheckListValue WHERE AccountId = @CurrentAccountId AND CheckListId=@Id
	
	End

	DELETE from @TempT_AL_CheckList WHERE ChecklistId = @Id
	
End

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
-------------------------------------------------------------------------------

Exec P_AL_GetAllocationPreferenceById @NewPresetDefId

Exec sp_xml_removedocument @handle