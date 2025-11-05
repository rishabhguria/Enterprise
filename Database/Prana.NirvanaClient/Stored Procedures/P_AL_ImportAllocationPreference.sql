
CREATE PROCEDURE [dbo].[P_AL_ImportAllocationPreference]
(
	@PrefXML nText	
)
AS

BEGIN TRY

DECLARE @intErrorCode INT

BEGIN TRAN

DECLARE @PreferenceID	INT
DECLARE @Id				INT
DECLARE @handle			INT
EXEC sp_xml_preparedocument @handle OUTPUT,@PrefXML 

-------------------------------------------------------------------------------
----------------- Creating #TempAllocationPreferenceDef tables ----------------
-------------------------------------------------------------------------------

CREATE TABLE #TempAllocationPreferenceDef
(
	[Id]						INT NOT NULL, 
	[Name]						NVARCHAR(50) NOT NULL,
	[CompanyId]					INT NOT NULL,	
	[AllocationBase]			INT NOT NULL,
	[MatchingRule]				INT NOT NULL,
	[MatchPortfolioPosition]	int NOT NULL,
	[PreferencedFundId]			INT NULL,
	[UpdateDateTime]			DATETIME NOT NULL,
	[ProrataDaysBack]			INT NULL,
	[PositionPriority]			INT NULL,
	[IsPrefVisible]				BIT NOT NULL
)

-------------------------------------------------------------------------------
--------------- Inserting into #TempAllocationPreferenceDef table -------------
-------------------------------------------------------------------------------
INSERT INTO #TempAllocationPreferenceDef
(
	[Id],
	[Name],
	[CompanyId],
	[AllocationBase],
	[MatchingRule],
	[MatchPortfolioPosition], 
	[PreferencedFundId],
	[UpdateDateTime],
	[ProrataDaysBack],
	[PositionPriority],
	[IsPrefVisible]
)
SELECT 
	OperationPreferenceId, OperationPreferenceName, CompanyId, 
	(
		SELECT	id 
		FROM	T_AL_AllocationBase AS B 
		WHERE	B.AllocationBase =xm.AllocationBase
	) AS AllocationBase,		
	(
		SELECT	id 
		FROM	T_AL_MatchingRule AS B 
		WHERE	B.MatchingRule =xm.MatchingRule
	) AS MatchingRule,
	(
		SELECT	id 
		FROM	T_AL_MatchPortfolioPosition AS B 
		WHERE	B.MatchPortfolioPosition =xm.MatchPortfolioPosition
	) AS MatchPortfolioPosition,
	CASE 
		WHEN	xm.PreferencedFundId=-1 THEN NULL
		ELSE	xm.PreferencedFundId 
	END AS PreferencedFundId,
	GETDATE(),
	xm.ProrataDaysBack AS ProrataDaysBack,
	CASE
		WHEN	xm.PositionPriority = -1 THEN ISNULL((SELECT MAX(PositionPriority) FROM T_AL_AllocationPreferenceDef),0) + 1
		ELSE	xm.PositionPriority
	END AS PositionPriority,
	xm.IsPrefVisible AS IsPrefVisible
FROM  OPENXML(@handle, '/AllocationOperationPreference',2)                                                                                                                                  
WITH
(
	OperationPreferenceId	INT				'/AllocationOperationPreference/OperationPreferenceId',
	OperationPreferenceName	NVARCHAR(50)	'/AllocationOperationPreference/OperationPreferenceName',
	CompanyId				INT				'/AllocationOperationPreference/CompanyId',
	AllocationBase			NVARCHAR(50)	'/AllocationOperationPreference/DefaultRule/BaseType',
	MatchingRule			NVARCHAR(50)	'/AllocationOperationPreference/DefaultRule/RuleType',
	MatchPortfolioPosition	NVARCHAR(50)	'/AllocationOperationPreference/DefaultRule/MatchClosingTransaction',
	PreferencedFundId		NVARCHAR(50)	'/AllocationOperationPreference/DefaultRule/PreferenceAccountId',
	ProrataDaysBack			INT				'/AllocationOperationPreference/DefaultRule/ProrataDaysBack',
	PositionPriority		INT				'/AllocationOperationPreference/PositionPrefId',
	IsPrefVisible			BIT				'/AllocationOperationPreference/IsPrefVisible'
) AS xm

-------------------------------------------------------------------------------
-- Inserting into T_AL_AllocationPreferenceDef table from temp table ----------
-------------------------------------------------------------------------------

INSERT INTO T_AL_AllocationPreferenceDef
	(
		[Name],
		[CompanyId],
		[AllocationBase],
		[MatchingRule],
		[MatchPortfolioPosition],
		[PreferencedFundId],
		[UpdateDateTime],
		[ProrataDaysBack], 
		[PositionPriority],
		[IsPrefVisible]
	)
SELECT
		tempTab.[Name],
		tempTab.[CompanyId],
		tempTab.[AllocationBase],
		tempTab.[MatchingRule],
		tempTab.[MatchPortfolioPosition], 
		tempTab.[PreferencedFundId],
		tempTab.[UpdateDateTime],
		tempTab.[ProrataDaysBack],
		tempTab.[PositionPriority],
		tempTab.[IsPrefVisible]
FROM	#TempAllocationPreferenceDef AS tempTab

SELECT @PreferenceID = SCOPE_IDENTITY()

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_Asset table from XML -----------------
-------------------------------------------------------------------------------

INSERT INTO T_AL_ProrataFundList	( [ChecklistId],[PreferenceId],[FundId] )
SELECT	-1, @PreferenceID, [FundId]
FROM  OPENXML(@handle, '/AllocationOperationPreference/DefaultRule/ProrataAccountList/Int32',2)
WITH
(		
	[FundId] INT '.'	
) AS xm

-------------------------------------------------------------------------------
---- Inserting into temp #TempT_AL_AllocationPreferenceData table from XML ----
-------------------------------------------------------------------------------

SELECT	@PreferenceID AS [PresetdefId], [FundId], [Value]
INTO	#TempT_AL_AllocationPreferenceData
FROM	OPENXML(@handle, '/AllocationOperationPreference/TargetPercentage/KeyValuePair',2)                                                                                                                                  
WITH
	(	
		[FundId]	INT				'Value/AccountId',
		[Value]		DECIMAL(32,19)	'Value/Value'
	) AS xm WHERE [Value]>0

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_Asset table from XML -----------------
-------------------------------------------------------------------------------

SELECT	[AllocationPrefDataId], [StrategyId],[Value]
INTO	#TempT_AL_StrategyValue	
FROM	OPENXML(@handle, '/AllocationOperationPreference/TargetPercentage/KeyValuePair/Value/StrategyValueList/StrategyValue',2)
WITH
	(	
		[AllocationPrefDataId]	INT				'../../AccountId',
		[StrategyId]			INT				'StrategyId',
		[Value]					DECIMAL(32,19)	'Value'	
	) AS xm WHERE [Value]>0

-------------------------------------------------------------------------------
-------- Updating tables from temp #TempT_AL_CheckList table from XML ---------
-------------------------------------------------------------------------------
DECLARE @NewPrefDataId INT
DECLARE @CurrentFundId INT

WHILE (SELECT Count(*) From #TempT_AL_AllocationPreferenceData) > 0
BEGIN

	SELECT TOP 1 @CurrentFundId = FundId FROM #TempT_AL_AllocationPreferenceData
	
	INSERT INTO T_AL_AllocationPreferenceData
	(
		[PresetDefId],[FundId],[Value]
	)
	SELECT 
			@PreferenceID,
			[FundId],
			[Value]
	FROM	#TempT_AL_AllocationPreferenceData 
	WHERE	FundId = @CurrentFundId

	SELECT	@NewPrefDataId =SCOPE_IDENTITY()

	INSERT INTO T_AL_StrategyValue([AllocationPrefDataId],[StrategyId],[Value])
	SELECT	@NewPrefDataId,
			[StrategyId],
			[Value]
	FROM	#TempT_AL_StrategyValue
	WHERE	AllocationPrefDataId = @CurrentFundId 

	DELETE FROM #TempT_AL_AllocationPreferenceData WHERE FundId = @CurrentFundId
	
End

-------------------------------------------------------------------------------
------------- Creating #TempT_AL_CheckList table for checkLists ---------------
-------------------------------------------------------------------------------

CREATE TABLE #TempT_AL_CheckList
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
	[ProrataFundList] XML NULL,
	[ProrataDaysBack] INT NULL,
	[OrderSideOperator] INT NOT NULL
)

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_CheckList table from XML -------------
-------------------------------------------------------------------------------

INSERT INTO #TempT_AL_CheckList
	( 
		[ChecklistId],
		[PresetDefId],
		[ExchangeOperator],
		[AssetOperator],
		[PROperator],
		[AllocationBase],
		[MatchingRule],
		[MatchPortfolioPosition],
		[PreferencedFundId],
		[ProrataDaysBack],
		[OrderSideOperator]
	)
SELECT 
		[ChecklistId],
		@PreferenceID, 
		(
			SELECT	id 
			FROM	T_AL_Operator AS B 
			WHERE	B.Operator = xm.ExchangeOperator
		) AS [ExchangeOperator], 
		(
			SELECT	id 
			FROM	T_AL_Operator AS B 
			WHERE	B.Operator = xm.AssetOperator
		) AS [AssetOperator], 
		(
			SELECT	id 
			FROM	T_AL_Operator AS B 
			WHERE	B.Operator = xm.PROperator
		) AS [PROperator],
		(
			SELECT	id 
			FROM	T_AL_AllocationBase AS B 
			WHERE	B.AllocationBase =xm.AllocationBase
		) AS AllocationBase,
		(
			SELECT	id 
			FROM	T_AL_MatchingRule AS B 
			WHERE	B.MatchingRule =xm.MatchingRule
		) AS MatchingRule, 
		(
			SELECT id 
			FROM T_AL_MatchPortfolioPosition AS B
			WHERE B.MatchPortfolioPosition=xm.MatchPortfolioPosition
		) AS MatchPortfolioPosition, 
		CASE 
			WHEN xm.PreferencedFundId=-1 THEN NULL
			ELSE xm.PreferencedFundId 
		END AS PreferencedFundId,
		[ProrataDaysBack],
		(
			SELECT	id 
			FROM	T_AL_Operator AS B 
			WHERE	B.Operator = xm.OrderSideOperator
		) AS [OrderSideOperator]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair',2)
WITH
(	
	[ChecklistId]				INT				'Value/ChecklistId',
	[ExchangeOperator]			NVARCHAR(50)	'Value/ExchangeOperator',
	[AssetOperator]				NVARCHAR(50)	'Value/AssetOperator',
	[PROperator]				NVARCHAR(50)	'Value/PROperator',
	[AllocationBase]			NVARCHAR(50)	'Value/Rule/BaseType',
	[MatchingRule]				NVARCHAR(50)	'Value/Rule/RuleType',
	[MatchPortfolioPosition]	NVARCHAR(50)	'Value/Rule/MatchClosingTransaction',
	[PreferencedFundId]			INT				'Value/Rule/PreferenceAccountId',
	[ProrataDaysBack]			INT				'Value/Rule/ProrataDaysBack',
	[OrderSideOperator]			NVARCHAR(50)	'Value/OrderSideOperator'
	
) AS xm

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_Asset table from XML -----------------
-------------------------------------------------------------------------------

SELECT	[ChecklistId],[AssetId]
INTO	#TempT_AL_Asset
FROM	OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/AssetList/Int32',2)
WITH
	(	
		[ChecklistId]	INT	'../../ChecklistId',
		[AssetId]		INT	'.'	
	) AS xm

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_Exchange table from XML --------------
-------------------------------------------------------------------------------

SELECT	[ChecklistId],[ExchangeId]
INTO	#TempT_AL_Exchange
FROM	OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/ExchangeList/Int32',2)
WITH
	(	
		[ChecklistId]	INT	'../../ChecklistId',
		[ExchangeId]	INT	'.'	
	) AS xm

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_OrderSide table from XML --------------
-------------------------------------------------------------------------------

SELECT	[ChecklistId],[OrderSideId]
INTO	#TempT_AL_OrderSide
FROM	OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/OrderSideList/String',2)
WITH
	(	
		[ChecklistId]	INT	'../../ChecklistId',
		[OrderSideId]	NVARCHAR(50)	'.'	
	) AS xm


-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_PR table from XML -----------------
-------------------------------------------------------------------------------

SELECT	[ChecklistId],[PR]
INTO	#TempT_AL_PR
FROM	OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/PRList/String',2)
WITH
	(	
		[ChecklistId]	INT				'../../ChecklistId',
		[PR]			NVARCHAR(50)	'.'	
	) AS xm

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_Asset table from XML -----------------
-------------------------------------------------------------------------------

SELECT	[ChecklistId],[FundId]
INTO	#TempT_AL_ProrataFundList
FROM	OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/Rule/ProrataAccountList/Int32',2)
WITH
	(	
		[ChecklistId]	INT	'../../../ChecklistId',
		[FundId]		INT	'.'	
	) AS xm


-------------------------------------------------------------------------------
-------- Creating #TempT_AL_AccountCheckListValue table for checkLists -----
-------------------------------------------------------------------------------

	CREATE TABLE #TempT_AL_AccountCheckListValue
(	
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

-------------------------------------------------------------------------------
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
------------- Creating #TempT_AL_StrategyChecklistValues table for checkLists -------------------
-------------------------------------------------------------------------------

CREATE TABLE #TempT_AL_StrategyChecklistValues
(
	[AccountCheckListId] INT NOT NULL,
	[StrategyId] INT NULL, 
	[Value] decimal(32,19) NULL	
)
-------------------------------------------------------------------------------
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
---------- Inserting into temp #TempT_AL_StrategyChecklistValues table from XML -----------------
-------------------------------------------------------------------------------

insert into #TempT_AL_StrategyChecklistValues	( [AccountCheckListId], [StrategyId],[Value] )
SELECT [AccountCheckListId], [StrategyId],[Value]
FROM  OPENXML(@handle, '/AllocationOperationPreference/CheckListWisePreference/KeyValuePair/Value/TargetPercentage/KeyValuePair/Value/StrategyValueList/StrategyValue',2)
WITH
(	
	[AccountCheckListId] int '../../AccountId',
	[StrategyId] int 'StrategyId',
	[Value] decimal(32,19) 'Value'	
) as xm where [Value]>0

------------------------------------------------------------------------------------------
-------- Inserting data into tables from temp #TempT_AL_CheckList table from XML ---------
------------------------------------------------------------------------------------------
DECLARE @NewChecklistId INT

WHILE (SELECT Count(*) FROM #TempT_AL_CheckList) > 0
BEGIN

	SELECT TOP 1 @Id = ChecklistId FROM #TempT_AL_CheckList
	
	INSERT INTO T_AL_CheckList
		(
			[PresetDefId],
			[ExchangeOperator],
			[AssetOperator],
			[PROperator],
			[AllocationBase],
			[MatchingRule],
			[MatchPortfolioPosition],
			[PreferencedFundId],
			[ProrataDaysBack],
			[OrderSideOperator]
		)
	SELECT 
			@PreferenceID,
			[ExchangeOperator],
			[AssetOperator],
			[PROperator],
			[AllocationBase],
			[MatchingRule],
			[MatchPortfolioPosition],
			[PreferencedFundId],
			[ProrataDaysBack],
			[OrderSideOperator]
	FROM	#TempT_AL_CheckList
	WHERE	ChecklistId = @Id
	
	SELECT @NewChecklistId =SCOPE_IDENTITY()

	INSERT INTO T_AL_Asset
		(
			[ChecklistId],
			[AssetId]
		)
	SELECT 
			@NewChecklistId,
			[AssetId]
	FROM	#TempT_AL_Asset 
	WHERE	ChecklistId = @Id AND AssetId IS NOT NULL

	INSERT INTO T_AL_Exchange
		(
			[ChecklistId],
			[ExchangeId]
		)
	SELECT	@NewChecklistId,
			[ExchangeId]
	FROM	#TempT_AL_Exchange
	WHERE	ChecklistId = @Id AND ExchangeId IS NOT NULL

	INSERT INTO T_AL_PR
		(
			[ChecklistId],
			[PR]
		)
	SELECT	@NewChecklistId,
			[PR]
	FROM	#TempT_AL_PR
	WHERE	ChecklistId = @Id AND PR IS NOT NULL

	INSERT INTO T_AL_ProrataFundList
		(
			[ChecklistId],
			[PreferenceId],
			[FundId]
		)
	SELECT	@NewChecklistId,
			@PreferenceID,
			[FundId]
	FROM	#TempT_AL_ProrataFundList
	WHERE	ChecklistId = @Id

	INSERT INTO T_AL_OrderSide
		(
			[ChecklistId],
			[OrderSideId]
		)
	SELECT 
			@NewChecklistId,
			[OrderSideId]
	FROM	#TempT_AL_OrderSide 
	WHERE	ChecklistId = @Id AND OrderSideId IS NOT NULL

	DECLARE @NewAccountCheckListValueId int
	DECLARE @CurrentAccountId int
	While (Select Count(*) From #TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id) > 0
	Begin

		Select Top 1 @CurrentAccountId = AccountId From #TempT_AL_AccountCheckListValue WHERE ChecklistId = @Id
	
		insert into T_AL_AccountCheckListValue
		(
			[CheckListId],[AccountId],[Value]
		)
		SELECT 
				@NewChecklistId,[AccountId],[Value]
		FROM  #TempT_AL_AccountCheckListValue WHERE AccountId = @CurrentAccountId AND ChecklistId = @Id
		SELECT @NewAccountCheckListValueId =SCOPE_IDENTITY()

		insert into T_AL_StrategyChecklistValues([AccountCheckListId],[StrategyId],[Value])
		SELECT @NewAccountCheckListValueId,[StrategyId],[Value]
		FROM  #TempT_AL_StrategyChecklistValues WHERE AccountCheckListId = @CurrentAccountId 
		
		DELETE from #TempT_AL_StrategyChecklistValues WHERE AccountCheckListId = @CurrentAccountId
		DELETE from #TempT_AL_AccountCheckListValue WHERE AccountId = @CurrentAccountId AND CheckListId=@Id
	
	End

	DELETE FROM #TempT_AL_CheckList WHERE ChecklistId = @Id
	
END

-------------------------------------------------------------------------------
-------------------------------------------------------------------------------

EXEC P_AL_GetAllocationPreferenceById @PreferenceID
COMMIT 
EXEC sp_xml_removedocument @handle
END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
    DECLARE @ErrorMessage	NVARCHAR(4000);
    DECLARE @ErrorSeverity	INT;
    DECLARE @ErrorState		INT;

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

