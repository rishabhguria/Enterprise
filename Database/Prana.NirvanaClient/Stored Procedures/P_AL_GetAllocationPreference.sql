
CREATE PROCEDURE [dbo].[P_AL_GetAllocationPreference]

AS

BEGIN TRY
BEGIN TRAN

SELECT 
	def.[Id],
	def.[Name],
	def.[CompanyId],
	def.[PositionPriority],
	def.[AllocationBase],
	def.[MatchingRule],
	def.[MatchPortfolioPosition],
	def.[PreferencedFundId],
	def.[UpdateDateTime],
	def.[IsPrefVisible],
	(
		select FundId as FundId
		from T_AL_ProrataFundList as TPFL
		where TPFL.checkListId=-1 and TPFL.PreferenceId= def.Id
		FOR XML AUTO, ELEMENTS, ROOT('ProrataFundList')
	) as ProrataFundList,def.ProrataDaysBack, 
	(
		SELECT PercentageData.[FundId], PercentageData.[Value],
		(	
			select [StrategyId], [Value]
			from T_AL_StrategyValue as Strategy
			where Strategy.[AllocationPrefDataId]=PercentageData.Id 
			FOR XML AUTO, ELEMENTS, ROOT('StrategyList')
		) as StrategyList
		
		FROM T_AL_AllocationPreferenceData as PercentageData 
		where PercentageData.PresetDefId = def.Id 
		FOR XML AUTO, TYPE, ELEMENTS, ROOT('PercentageDataList'), XMLSCHEMA
	) as PercentageDataList,
	(
		SELECT 
		[CheckListId],[PresetDefId],	
		[ExchangeOperator],
		(
			select ExchangeId
			from T_AL_Exchange as Exchange
			where Exchange.checkListId=CheckList.checkListId 
			FOR XML AUTO, ELEMENTS, ROOT('ExchangeList')
		) as ExchangeList,
		[AssetOperator], 
		(
			select AssetId
			from T_AL_Asset as Asset
			where Asset.checkListId=CheckList.checkListId 
			FOR XML AUTO, ELEMENTS, ROOT('AssetList')
		) as AssetList,
		[PROperator],
		(
			select PR as PRId
			from T_AL_PR as TPR
			where TPR.checkListId=CheckList.checkListId 
			FOR XML AUTO, ELEMENTS, ROOT('PRList')
		) as PRList,
		[AllocationBase],[MatchingRule],
		[MatchPortfolioPosition],
			[PreferencedFundId],
		(
			select FundId as FundId
			from T_AL_ProrataFundList as TPFL
			where TPFL.checkListId=CheckList.checkListId
			FOR XML AUTO, ELEMENTS, ROOT('ProrataFundList')
		) as ProrataFundList,
		[ProrataDaysBack],
		[OrderSideOperator],
		(
			select OrderSideId
			from T_AL_OrderSide as OrderSide
			where OrderSide.checkListId=CheckList.checkListId 
			FOR XML AUTO, ELEMENTS, ROOT('OrderSideList')
		) as OrderSideList,
		(
			SELECT TargetPercentageData.[AccountId], TargetPercentageData.[Value],
			(	
				SELECT [StrategyId], [Value]
				from T_AL_StrategyChecklistValues as StrategyData
				where StrategyData.AccountCheckListId = TargetPercentageData.Id 
				FOR XML AUTO, ELEMENTS, ROOT('StrategyCheckListValue')
			) as StrategyCheckListValue
			FROM T_AL_AccountCheckListValue as TargetPercentageData 
			where TargetPercentageData.CheckListId = CheckList.CheckListId 
			FOR XML AUTO, ELEMENTS, ROOT('TargetPercentageCheckListData'), XMLSCHEMA
		) as TargetPercentageCheckListData
		FROM T_AL_CheckList as CheckList
		where CheckList.PresetDefId = def.Id 
		FOR XML AUTO, TYPE, ELEMENTS, ROOT('CheckListCollection'), XMLSCHEMA
	) as CheckListCollection

	FROM T_AL_AllocationPreferenceDef AS def
	ORDER BY def.PositionPriority ASC

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