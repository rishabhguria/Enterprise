CREATE PROCEDURE [dbo].[P_SaveTradingRulesPreferences](
	@Xml XML
	,@id INT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY
	BEGIN TRANSACTION TRAN1

	DECLARE @handle INT

	EXEC sp_xml_preparedocument @handle OUTPUT
		,@Xml

	CREATE TABLE #XmlItemTradingRulesPrefs (
		IsOversellTradingRule BIT
		,IsOverbuyTradingRule BIT
		,IsUnallocatedTradeAlert BIT
		,IsFatFingerTradingRule BIT
		,IsDuplicateTradeAlert BIT
		,IsPendingNewTradeAlert BIT
		,DefineFatFingerValue FLOAT
		,DuplicateTradeAlertTime INT
		,PendingNewOrderAlertTime INT
		,FatFingerAccountOrMasterFund INT
		,IsAbsoluteAmountOrDefinePercent INT
		,IsInMarketIncluded BIT
		,IsSharesOutstandingRule BIT
		,SharesOutstandingAccountOrMF INT
		,SharesOutstandingPercent FLOAT
		)

		INSERT INTO #XmlItemTradingRulesPrefs (
		IsOversellTradingRule
		,IsOverbuyTradingRule
		,IsUnallocatedTradeAlert
		,IsFatFingerTradingRule
		,IsDuplicateTradeAlert
		,IsPendingNewTradeAlert
		,DefineFatFingerValue
		,DuplicateTradeAlertTime
		,PendingNewOrderAlertTime
		,FatFingerAccountOrMasterFund
		,IsAbsoluteAmountOrDefinePercent
		,IsInMarketIncluded
		,IsSharesOutstandingRule
		,SharesOutstandingAccountOrMF
		,SharesOutstandingPercent
		)
	SELECT IsOversellTradingRule
		,IsOverbuyTradingRule
		,IsUnallocatedTradeAlert
		,IsFatFingerTradingRule
		,IsDuplicateTradeAlert
		,IsPendingNewTradeAlert
		,DefineFatFingerValue
		,DuplicateTradeAlertTime
		,PendingNewOrderAlertTime
		,FatFingerAccountOrMasterFund
		,IsAbsoluteAmountOrDefinePercent
		,IsInMarketIncluded
		,IsSharesOutstandingRule
		,SharesOutstandingAccountOrMF
		,SharesOutstandingPercent
	FROM OPENXML(@handle, '//TradingTicketRulesPreferences', 2) WITH (
		IsOversellTradingRule BIT 'IsOversellTradingRule'
		,IsOverbuyTradingRule BIT 'IsOverbuyTradingRule'
		,IsUnallocatedTradeAlert BIT 'IsUnallocatedTradeAlert'
		,IsFatFingerTradingRule BIT 'IsFatFingerTradingRule'
		,IsDuplicateTradeAlert BIT 'IsDuplicateTradeAlert'
		,IsPendingNewTradeAlert BIT 'IsPendingNewTradeAlert'
		,DefineFatFingerValue FLOAT 'DefineFatFingerValue'
		,DuplicateTradeAlertTime INT 'DuplicateTradeAlertTime'
		,PendingNewOrderAlertTime INT 'PendingNewOrderAlertTime'
		,FatFingerAccountOrMasterFund INT 'FatFingerAccountOrMasterFund'
	    ,IsAbsoluteAmountOrDefinePercent INT 'IsAbsoluteAmountOrDefinePercent'
		,IsInMarketIncluded BIT 'IsInMarketIncluded'
		,IsSharesOutstandingRule BIT 'IsSharesOutstandingRule'
		,SharesOutstandingAccountOrMF INT 'SharesOutstandingAccOrMF'
		,SharesOutstandingPercent FLOAT 'SharesOutstandingValue'
			)

	DELETE
	FROM T_TradingRulesPreferences
	WHERE CompanyID = @id;

	INSERT INTO T_TradingRulesPreferences (
		CompanyID
		,IsOversellTradingRule
		,IsOverbuyTradingRule
		,IsUnallocatedTradeAlert
		,IsFatFingerTradingRule
		,IsDuplicateTradeAlert
		,IsPendingNewTradeAlert
		,DefineFatFingerValue
		,DuplicateTradeAlertTime
		,PendingNewOrderAlertTime
		,FatFingerAccountOrMasterFund
		,IsAbsoluteAmountOrDefinePercent
		,IsInMarketIncluded
		,IsSharesOutstandingRule
		,SharesOutstandingAccountOrMF
		,SharesOutstandingPercent
		)
	SELECT @id
		,IsOversellTradingRule
		,IsOverbuyTradingRule
		,IsUnallocatedTradeAlert
		,IsFatFingerTradingRule
		,IsDuplicateTradeAlert
		,IsPendingNewTradeAlert
		,DefineFatFingerValue
		,DuplicateTradeAlertTime
		,PendingNewOrderAlertTime
		,FatFingerAccountOrMasterFund
		,IsAbsoluteAmountOrDefinePercent
		,IsInMarketIncluded
		,IsSharesOutstandingRule
		,SharesOutstandingAccountOrMF
		,SharesOutstandingPercent

FROM #XmlItemTradingRulesPrefs

	EXEC sp_xml_removedocument @handle

	DROP TABLE #XmlItemTradingRulesPrefs

	COMMIT TRANSACTION TRAN1
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();

	ROLLBACK TRANSACTION TRAN1
END CATCH;